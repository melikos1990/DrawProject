using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using SMARTII.Domain.Common;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Service.Notification.Facade
{
    public class OfficialEmalEffectiveDataFacade : IOfficialEmalEffectiveDataFacade
    {

        private readonly IMasterAggregate _MasterAggregate;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly INotificationAggregate _NotificationAggregate;


        public OfficialEmalEffectiveDataFacade(IMasterAggregate MasterAggregate,
                                               ICommonAggregate CommonAggregate,
                                               INotificationAggregate NotificationAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _CommonAggregate = CommonAggregate;
            _NotificationAggregate = NotificationAggregate;
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Create(List<(OfficialEmailEffectivePayload, OfficialEmailHistory)> data)
        {
            new FileProcessInvoker((context) =>
            {

                var errorMsgIDs = new List<string>();
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TimeSpan(0, 30, 0), TransactionScopeAsyncFlowOption.Enabled))
                {
                    foreach (var item in data)
                    {

                        try
                        {
                            //將附件放進站台
                            var path = FileSaverUtility.SaveMailFiles(context, item.Item1);

                            item.Item1.FilePath = path;

                            _NotificationAggregate.OfficialEmailEffectivePayload_T1_T2_.Add(item.Item1);

                            _NotificationAggregate.OfficialEmailHistory_T1_T2_.Add(item.Item2);
                        }

                        catch (Exception ex)
                        {
                            _CommonAggregate.Logger.Error($"【解析官網來信】 寫入DB失敗{item.Item1.MessageID}，錯誤訊息: {ex.ToString()}。");
                            errorMsgIDs.Add(item.Item1.MessageID);
                        }
                    }
                    scope.Complete();
                }

                if(errorMsgIDs.Count > 0)
                    _CommonAggregate.Loggers["Email"].Error($"【解析官網來信】 寫入DB失敗 MessageID : {string.Join(", ", errorMsgIDs.ToArray())}");
            });

        }
    }
}
