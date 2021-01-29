using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using MoreLinq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.DI;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;

namespace SMARTII.Service.Notification.Service
{
    public class OfficialEmailEffectiveDataService : IEmailService
    {
        private readonly ICommonAggregate _CommonAggregate;
        private readonly ISystemAggregate _SystemAggregate;
        private readonly IMasterAggregate _MasterAggregate;
        private readonly INotificationAggregate _NotificationAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IOfficialEmalEffectiveDataFacade _OfficialEmalEffectiveDataFacade;
        private readonly IIndex<string, IEmailMailProtocolFactory> _IEmailMailProtocolFactory;
        private readonly INotificationPersonalFacade _NotificationPersonalFacade;


        public OfficialEmailEffectiveDataService(ICommonAggregate CommonAggregate,
                                                 IMasterAggregate MasterAggregate,
                                                 ISystemAggregate SystemAggregate,
                                                 INotificationAggregate NotificationAggregate,
                                                 IOrganizationAggregate OrganizationAggregate,
                                                 IOfficialEmalEffectiveDataFacade OfficialEmalEffectiveDataFacade,
                                                 IIndex<string, IEmailMailProtocolFactory> IEmailMailProtocolFactory,
                                                 INotificationPersonalFacade NotificationPersonalFacade)
        {
            _CommonAggregate = CommonAggregate;
            _MasterAggregate = MasterAggregate;
            _SystemAggregate = SystemAggregate;
            _NotificationAggregate = NotificationAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _IEmailMailProtocolFactory = IEmailMailProtocolFactory;
            _OfficialEmalEffectiveDataFacade = OfficialEmalEffectiveDataFacade;
            _NotificationPersonalFacade = NotificationPersonalFacade;
        }
        /// <summary>
        /// 取得實體信內容並存儲至DB
        /// </summary>
        public void ReceiveEmail()
        {
            _CommonAggregate.Logger.Info("【解析官網來信】  開始排程");
            try
            {
                var now = DateTime.Now;
                var con = new MSSQLCondition<OFFICIAL_EMAIL_GROUP>();
                con.IncludeBy(x => x.USER);
                con.And(x => x.IS_ENABLED == true);
                con.And(x => x.ALLOW_RECEIVE == true);
                //取出官網來信清單
                var dataList = _NotificationAggregate.OfficialEmailGroup_T1_T2_.GetList(con);

                _CommonAggregate.Logger.Info($"【解析官網來信】  撈出共 {dataList.Count()} 個官網來信。");

                // 依BU節點切割 來源信箱
                var groups = dataList.GroupBy(x => x.NodeID);

                // 取得 所有BU的節點
                var buIDs = dataList.Select(x => x.NodeID).Distinct();
                var buList = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.GetList(x => buIDs.Contains(x.NODE_ID));

                // 依BU Loop
                foreach (var group in groups)
                {
                    try
                    {
                        var conhis = new MSSQLCondition<OFFICIAL_EMAIL_HISTORY>();
                        conhis.And(x => x.NODE_ID == group.Key);

                        // 比對清單
                        var historyList = _NotificationAggregate.OfficialEmailHistory_T1_T2_.GetList(conhis);
                        var historygroupList = historyList.ToDictionary(x => x.MessageID);

                        string NodeKey = buList.Where(x => x.NodeID == group.Key).FirstOrDefault().NodeKey;
                        string NodeName = buList.Where(x => x.NodeID == group.Key).FirstOrDefault().Name;

                        _CommonAggregate.Logger.Info($"【解析官網來信】，目前要撈出BU代號 : {NodeKey}，BU名稱 : {NodeName}，的資料。");
                        // 依信箱來源 Loop
                        foreach (var item in group)
                        {
                            try
                            {
                                _CommonAggregate.Logger.Info($"【解析官網來信】，目前要撈出信箱 : {item.OfficialEmail} 的資料。");

                                //從實體信比對出需要新增的資料
                                IEmailMailProtocolFactory service = _IEmailMailProtocolFactory.TryGetService(item.MailProtocolType.GetDescription(), EssentialCache.MailProtocolKeyValue.POP3);
                                var addList = service.LoginMailProtocol(item, historygroupList, NodeKey);

                                _CommonAggregate.Logger.Info($"【解析官網來信】，實體信撈出共 {addList.Count()} 個 。");

                                if (addList.Any())
                                {
                                    _CommonAggregate.Logger.Info($"【解析官網來信】，將實體信件新增至Table OfficialEmailEffectiveData and OfficialEmailHistory。");
                                    //新增DATA
                                    _OfficialEmalEffectiveDataFacade.Create(addList);


                                    //根據待通知對象 , 寫入 (PERSONAL_NOTIFICATION)
                                    List<PersonalNotification> personList = new List<PersonalNotification>();

                                    var officialDatas = addList.Select(x => x.Item1);

                                    _CommonAggregate.Logger.Info($"【解析官網來信】，根據待通知對象，寫入 PERSONAL_NOTIFICATION。");
                                    officialDatas.ForEach(data =>
                                    {
                                        var content = string.Format(NotificationFormatted.MailIncoming, NodeName, data.Body);
                                        content = content.Length > 4000 ? content.Substring(0, 4000) : content;
                                    });
                                }

                                _CommonAggregate.Logger.Info($"【解析官網來信】，刪除OfficialEmailHistory超過時效的資料");
                                //刪除history
                                var eliminateDateTime = now.AddDays(-item.KeepDay);
                                var conDelete = new MSSQLCondition<OFFICIAL_EMAIL_HISTORY>(x => x.DOWNLOAD_DATETIME < eliminateDateTime && x.EMAIL_GROUP_ID == item.ID);
                                _NotificationAggregate.OfficialEmailHistory_T1_.RemoveRange(conDelete);

                            }
                            catch (Exception ex)
                            {
                                _CommonAggregate.Logger.Error(ex.ToString());
                                _CommonAggregate.Loggers["Email"].Error($"【解析官網來信】信箱失敗, 原因 : {ex.Message}");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        _CommonAggregate.Logger.Error(ex.ToString());
                        _CommonAggregate.Loggers["Email"].Error($"【解析官網來信】BU失敗, 原因 : {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【解析官網來信】整批失敗, 原因 : {ex.Message}");
            }

            _CommonAggregate.Logger.Info("【解析官網來信】  結束排程");
        }
    }
}
