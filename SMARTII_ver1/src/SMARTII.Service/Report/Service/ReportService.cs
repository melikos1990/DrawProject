using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using Autofac.Features.Indexed;
using Ionic.Zip;
using MoreLinq;
using MultipartDataMediaFormatter.Infrastructure;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;
using SMARTII.Resource.Tag;
using SMARTII.Service.Cache;
using SMARTII.Service.Organization.Provider;
using static SMARTII.Domain.Cache.EssentialCache;

namespace SMARTII.Service.Report.Service
{
    public class ReportService : IReportService
    {
        private readonly ICaseAggregate _CaseAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;
        private readonly IPPCLIFEFactory _PPCLIFEFactory;
        private readonly IColdStoneFactory _ColdStoneFactory;
        private readonly IASOFactory _ASOFactory;
        private readonly IEShopFactory _EShopFactory;
        private readonly IICCFactory _ICCFactory;
        private readonly I21Factory _I21Factory;
        private readonly IMisterDonutFactory _MisterDonutFactory;
        private readonly IOpenPointFactory _OpenPointFactory;
        private readonly IBatchReportProvider _BatchReportProvider;
        private readonly IIndex<string, IReportProvider> _ReportProviders;
        private readonly HeaderQuarterNodeProcessProvider _HeaderQuarterNodeProcessProvider;
        private readonly IIndex<NotificationType, INotificationProvider> _NotificationProviders;
        private readonly ICommonAggregate _CommonAggregate;

        public ReportService(ICaseAggregate CaseAggregate,
                             IOrganizationAggregate OrganizationAggregate,
                             IPPCLIFEFactory PPCLIFEFactory,
                             IColdStoneFactory ColdStoneFactory,
                             IASOFactory ASOFactory,
                             IEShopFactory EShopFactory,
                             IICCFactory ICCFactory,
                             I21Factory _21Factory,
                             IMisterDonutFactory MisterDonutFactory,
                             IOpenPointFactory OpenPointFactory,
                             IBatchReportProvider BatchReportProvider,
                             IIndex<string, IReportProvider> ReportProviders,
                             HeaderQuarterNodeProcessProvider HeaderQuarterNodeProcessProvider,
                             IIndex<NotificationType, INotificationProvider> NotificationProviders,
                             ICommonAggregate CommonAggregate)
        {
            _CaseAggregate = CaseAggregate;
            _OrganizationAggregate = OrganizationAggregate;
            _PPCLIFEFactory = PPCLIFEFactory;
            _ColdStoneFactory = ColdStoneFactory;
            _ASOFactory = ASOFactory;
            _EShopFactory = EShopFactory;
            _ICCFactory = ICCFactory;
            _I21Factory = _21Factory;
            _MisterDonutFactory = MisterDonutFactory;
            _OpenPointFactory = OpenPointFactory;
            _ReportProviders = ReportProviders;
            _BatchReportProvider = BatchReportProvider;
            _HeaderQuarterNodeProcessProvider = HeaderQuarterNodeProcessProvider;
            _NotificationProviders = NotificationProviders;
            _CommonAggregate = CommonAggregate;
        }



        /// <summary>
        /// 輸出報表
        /// </summary>
        /// <param name="caseId">案件編號</param>
        /// <param name="invoiceId">反應單單號</param>
        /// <returns></returns>
        public HttpFile GetComplaintReport(string caseId, string invoiceId, bool isEncrypt = false)
        {
            try
            {
                // 取得Case資料
                var con = new MSSQLCondition<CASE>(x => x.CASE_ID == caseId);
                con.IncludeBy(x => x.CASE_COMPLAINED_USER);

                var data = _CaseAggregate.Case_T1_T2_.Get(con);

                if (data == null)
                    throw new Exception(Resource.Tag.Common_lang.REPORT_ERROR);

                if (data.CaseComplainedUsers.Any() == false)
                    throw new Exception(Resource.Tag.Common_lang.COMPLAINEDUSERS_ERROR);

                // 取得 BU 三碼
                var term = (HeaderQuarterTerm)_HeaderQuarterNodeProcessProvider
                            .GetTerm(data.NodeID, data.OrganizationType);

                // 取得NodeKey (BU三碼識別直)
                var service = _ReportProviders[term.NodeKey];

                // 產生待輸出物件
                var payload = service.GeneratorPayload(caseId, invoiceId);

                if (isEncrypt)
                {
                    // 取得附件密碼
                    DataStorage.CaseInvoiceSheetPasswordDict.TryGetValue(term.NodeKey, out var password);

                    payload.Password = password;
                }
                //產生檔名
                var complaint = data.CaseComplainedUsers.Where(x => x.CaseComplainedUserType == CaseComplainedUserType.Responsibility);
                string fileName = "";
                if (complaint != null && complaint.First().UnitType == UnitType.Store)
                {
                    if (term.NodeKey == BusinessKeyValue._21Century)
                    {
                        fileName = complaint.First().NodeName + invoiceId;
                    }
                    else
                    {
                        fileName = complaint.First().ParentName + "-" + complaint.First().NodeName + invoiceId;
                    }
                }
                else if (complaint != null && complaint.First().UnitType == UnitType.Organization)
                {
                    if (term.NodeKey == BusinessKeyValue._21Century)
                    {
                        fileName = complaint.First().NodeName + invoiceId;
                    }
                    else
                    {
                        fileName = invoiceId;
                    }
                }
                payload.fileName = fileName;


                // 產生實體報表
                var file = service.ComplaintedReport(payload);

                return file;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 案件提醒通知報表
        /// </summary>
        public void CaseRemindNotification()
        {

            try
            {

                _CommonAggregate.Logger.Info($"[案件提醒通知] 開始執行 CaseRemindNotification");


                var now = DateTime.Now;
                var mailTitle = $"{DateTime.Now.ToString("yyyyMMdd")}_案件提醒逾時未完成";
                var reportName = $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}_案件提醒逾時未完成.xlsx";
                var mediaType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName)).MediaType;

                var con = new MSSQLCondition<CASE_REMIND>(x => x.IS_CONFIRM == false && x.IS_NOTIFCATED == false && x.ACTIVE_END_DAETTIME <= now);
                con.IncludeBy(x => x.CASE);
                con.IncludeBy(x => x.CASE.CASE_WARNING);

                var caseReminds = _CaseAggregate.CaseRemind_T1_T2_.GetList(con).ToList();

                _CommonAggregate.Logger.Info($"[案件提醒通知] 取得 案件提醒逾時未完成, 總筆數: {caseReminds.Count}");

                // 取得 CC GroupID
                var ccGroupIDs = caseReminds.Select(x => x.Case.GroupID).Distinct();

                // 取得通知人員
                var nodeJobCon = new MSSQLCondition<NODE_JOB>(x => ccGroupIDs.Contains(x.NODE_ID) && x.ORGANIZATION_TYPE == (int)OrganizationType.CallCenter);
                nodeJobCon.IncludeBy(x => x.USER);

                var jobPositions = _OrganizationAggregate.JobPosition_T1_T2_.GetListOfSpecific(
                                nodeJobCon,
                                x => new JobPosition
                                {
                                    NodeID = x.NODE_ID,
                                    Users = x.USER.Select(g => new User { Name = g.NAME, Email = g.EMAIL, UserID = g.USER_ID }).ToList()
                                });



                #region 依 案件Group為群組匯出報表並寄出


                var caseRemindGroup = caseReminds.GroupBy(x => x.Case.GroupID);

                // 依 案件Group為群組匯出報表並寄出
                foreach (var group in caseRemindGroup)
                {
                    try
                    {
                        var caseRemindList = group.ToList();

                        _CommonAggregate.Logger.Info($"[案件提醒通知] 通知群組節點ID: {group.Key}  筆數: {caseRemindList.Count}");

                        // 產出報表
                        var @byte = _BatchReportProvider.GenerateCaseRemindReport(caseRemindList);

                        var users = jobPositions.Where(x => x.NodeID == group.Key)?
                                                .SelectMany(x => x.Users)
                                                .Where(user => !string.IsNullOrEmpty(user.Email))?
                                                .DistinctBy(x => x.UserID) ?? null;

                        if (users != null)
                        {
                            //收件者
                            var receiver = users.Select(x =>
                            {

                                _CommonAggregate.Logger.Info($"[案件提醒通知] 對象:{x.Name} Email: {x.Email}");

                                return new ConcatableUser { Email = x.Email, UserName = x.Name };
                            }).ToList();

                            //CC系統人員
                            var ccReceiver = new List<ConcatableUser>()
                            {
                                new ConcatableUser()
                                {
                                    Email = GlobalizationCache.Instance.AdminMailReceiverAddress
                                }
                            };

                            #region 組合 payload 寄信

                            var emailPayload = new EmailPayload()
                            {
                                Title = mailTitle,
                                Sender = new ConcatableUser() { Email = GlobalizationCache.Instance.AdminMailAddress, UserName = GlobalizationCache.APName },
                                Receiver = receiver,
                                Cc = ccReceiver,
                                Attachments = new List<HttpFile>() { new HttpFile(reportName, mediaType, @byte) }
                            };


                            _NotificationProviders[NotificationType.Email].Send(
                                payload: emailPayload
                            );

                            // 更新狀態
                            this.SendCaseRemindNotificationSuccess(caseRemindList.Select(x => x.ID).ToList());

                            _CommonAggregate.Logger.Info("[案件提醒通知] 寄信成功");
                            #endregion


                        }
                    }
                    catch (Exception ex)
                    {
                        _CommonAggregate.Logger.Error(ex);
                        _CommonAggregate.Logger.Info("[案件提醒通知] 寄信失敗");
                    }
                }

                #endregion

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);
                _CommonAggregate.Logger.Info("[案件提醒通知] 執行 CaseRemindNotification 失敗");
            }

        }

        #region 報表


        #region 21
        /// <summary>
        /// 21世紀來電紀錄
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="password"></param>
        public async void Send21OnCallExcel(DateTime start, DateTime end, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【21世紀日報表】  準備進行排程 , 時間 : {now.ToString()}。");
                var @byte = await _I21Factory.GenerateOnCallExcel(start, end);

                _CommonAggregate.Logger.Info($"【21世紀日報表】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == BatchValue._21CenterGroupDaily);
                _CommonAggregate.Logger.Info($"【21世紀日報表】  取得寄信人清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                var senderList = caseAssigmentGroupList.SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();

                _CommonAggregate.Logger.Info($"【21世紀日報表】  取得寄信人清單，成功。");
                //加密壓縮Zip
                string fileName = "21世紀來電紀錄-S5.xlsx";
                var zipbyte = @byte.ConvertZipByte(fileName, password);
                #region 組合 payload 寄信
                //主旨
                var mailTitle = $"21世紀日報表{end.ToString("yyyy/MM/dd")}";
                var reportName1 = $"21世紀來電紀錄-S5.zip";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;
                //檔案 
                var fileList = new List<HttpFile>();
                fileList.Add(new HttpFile(reportName1, mediaType1, zipbyte));

                CreatMailPayload(mailTitle, senderList, fileList);

                _CommonAggregate.Logger.Info($"【21世紀日報表】  寄信成功。");

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【21世紀日報表】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【21世紀日報表】失敗，原因 : {ex.Message}");
            }

            #endregion
        }

        #endregion



        #region ASO
        /// <summary>
        /// ASO日報表寄送
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="password"></param>
        public async void SendASOOnCallExcel(DateTime start, DateTime end, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【ASO日報表寄送】  準備進行排程 , 時間 : {now.ToString()}。");

                _CommonAggregate.Logger.Info($"【ASO日報表寄送】  開始取得 ASO來電紀錄。");
                var @byte1 = await _ASOFactory.GenerateOnCallExcel(start, end);

                _CommonAggregate.Logger.Info($"【ASO日報表寄送】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == BatchValue.ASOGroupDaily);
                _CommonAggregate.Logger.Info($"【ASO日報表寄送】  取得寄信人清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                var senderList = caseAssigmentGroupList.SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();

                _CommonAggregate.Logger.Info($"【ASO日報表寄送】  取得寄信人清單，成功。");
                //加密壓縮Zip
                string fileName = "ASO 0800來電紀錄-S5.xlsx";
                var zipbyte = @byte1.ConvertZipByte(fileName, password);

                #region 組合 payload 寄信
                //主旨
                var mailTitle = $"ASO日報表寄送{end.ToString("yyyy/MM/dd")}";
                var reportName1 = $"ASO 0800來電紀錄-S5.zip";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;
                //檔案 
                var fileList = new List<HttpFile>();
                fileList.Add(new HttpFile(reportName1, mediaType1, zipbyte));


                CreatMailPayload(mailTitle, senderList, fileList);

                _CommonAggregate.Logger.Info($"【ASO日報表寄送】  寄信成功。");

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【ASO日報表寄送】失敗，原因 :" + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【ASO日報表寄送】失敗，原因 : {ex.Message}");
            }


            #endregion

        }

        #endregion

        #region OpenPoint

        public async void SendOpenPointOnCallExcel(DateTime start, DateTime end, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【OpenPoint日報表】  準備進行排程 , 時間 : {now.ToString()}。");
                var @byte1 = await _OpenPointFactory.GetOnCallExcel(start, end);

                _CommonAggregate.Logger.Info($"【OpenPoint日報表】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == BatchValue.OpenPointGroupDaily);
                _CommonAggregate.Logger.Info($"【OpenPoint日報表】  取得寄信人清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                var senderList = caseAssigmentGroupList.SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();


                _CommonAggregate.Logger.Info($"【OpenPoint日報表】  取得寄信人清單，成功。");

                //加密壓縮Zip
                string fileName = "OPENPOINT來電紀錄-S5.xlsx";
                var zipbyte = @byte1.ConvertZipByte(fileName, password);

                #region 組合 payload 寄信
                //主旨
                var mailTitle = $"OpenPoint日報表{end.ToString("yyyy/MM/dd")}";

                var reportName1 = $"OPENPOINT來電紀錄-S5.zip";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;
                //檔案 
                var fileList = new List<HttpFile>();
                fileList.Add(new HttpFile(reportName1, mediaType1, zipbyte));

                CreatMailPayload(mailTitle, senderList, fileList);

                _CommonAggregate.Logger.Info($"【OpenPoint日報表】  寄信成功。");

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【OpenPoint日報表】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【OpenPoint日報表】失敗，原因 : {ex.Message}");
            }


            #endregion

        }



        #endregion

        #region ColdStone
        /// <summary>
        /// Batch 酷聖石0800客服日報表 寄信
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="password"></param>
        public async void SendColdStoneOnCallExcel(DateTime start, DateTime end, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【酷聖石0800客服日報表】  準備進行排程 , 時間 : {now.ToString()}。");
                var @byte1 = await _ColdStoneFactory.GetOnCallExcel(start, end);

                _CommonAggregate.Logger.Info($"【酷聖石0800客服日報表】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == BatchValue.ColdStoneGroupDaily);
                _CommonAggregate.Logger.Info($"【酷聖石0800客服日報表】  取得寄信人清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                var senderList = caseAssigmentGroupList.SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();


                _CommonAggregate.Logger.Info($"【酷聖石0800客服日報表】  取得寄信人清單，成功。");

                //加密壓縮Zip
                string fileName = "酷聖石0800客服 來電紀錄-S5.xlsx";
                var zipbyte = @byte1.ConvertZipByte(fileName, password);

                #region 組合 payload 寄信
                //主旨
                var mailTitle = $"酷聖石0800客服日報表{end.ToString("yyyy/MM/dd")}";

                var reportName1 = $"酷聖石0800客服 來電紀錄-S5.zip";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;
                //檔案 
                var fileList = new List<HttpFile>();
                fileList.Add(new HttpFile(reportName1, mediaType1, zipbyte));

                CreatMailPayload(mailTitle, senderList, fileList);

                _CommonAggregate.Logger.Info($"【酷聖石0800客服日報表】  寄信成功。");
                #endregion

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【酷聖石0800客服日報表】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【酷聖石0800客服日報表】失敗，原因 : {ex.Message}");
            }


        }

        #endregion


        #region Dount

        public async void SendDonutOnCallExcel(DateTime start, DateTime end, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【多拿滋日報表】  準備進行排程 , 時間 : {now.ToString()}。");
                var @byte1 = await _MisterDonutFactory.GetOnCallExcel(start, end);


                _CommonAggregate.Logger.Info($"【多拿滋日報表】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == BatchValue.MisterDonutGroupDaily);
                _CommonAggregate.Logger.Info($"【多拿滋日報表】  取得寄信人清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                var senderList = caseAssigmentGroupList.SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();


                _CommonAggregate.Logger.Info($"【多拿滋日報表】  取得寄信人清單，成功。");

                //加密壓縮Zip
                string fileName = "多拿滋來電紀錄-S5.xlsx";
                var zipbyte = @byte1.ConvertZipByte(fileName, password);

                #region 組合 payload 寄信
                //主旨
                var mailTitle = $"多拿滋日報表{end.ToString("yyyy/MM/dd")}";

                var reportName1 = $"多拿滋來電紀錄-S5.zip";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;
                //檔案 
                var fileList = new List<HttpFile>();
                fileList.Add(new HttpFile(reportName1, mediaType1, zipbyte));

                CreatMailPayload(mailTitle, senderList, fileList);

                _CommonAggregate.Logger.Info($"【多拿滋日報表】  寄信成功。");


                #endregion

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【多拿滋日報表】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【多拿滋日報表】失敗，原因 : {ex.Message}");
            }
        }

        #endregion


        #region Ehop

        public async void SendEhopOnCallExcel(DateTime start, DateTime end, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【統一藥品eShop日報表】  準備進行排程 , 時間 : {now.ToString()}。");
                var @byte1 = await _EShopFactory.GenerateOnCallExcel(start, end);

                _CommonAggregate.Logger.Info($"【統一藥品eShop日報表】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == BatchValue.EShopGroupDaily);
                _CommonAggregate.Logger.Info($"【統一藥品eShop日報表】  取得寄信人清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                var senderList = caseAssigmentGroupList.SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();


                _CommonAggregate.Logger.Info($"【統一藥品eShop日報表】  取得寄信人清單，成功。");


                //加密壓縮Zip
                string fileName = $"統一藥品eShop來電紀錄-S5.xlsx";
                var zipbyte = @byte1.ConvertZipByte(fileName, password);

                #region 組合 payload 寄信
                //主旨
                var mailTitle = $"統一藥品eShop日報表{end.ToString("yyyy/MM/dd")}";

                var reportName1 = $"統一藥品eShop來電紀錄-S5.zip";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;
                //檔案 
                var fileList = new List<HttpFile>();
                fileList.Add(new HttpFile(reportName1, mediaType1, zipbyte));

                CreatMailPayload(mailTitle, senderList, fileList);

                _CommonAggregate.Logger.Info($"【統一藥品eShop日報表】  寄信成功。");


                #endregion
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【統一藥品eShop日報表】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【統一藥品eShop日報表】失敗，原因 : {ex.Message}");
            }
        }

        #endregion


        #region ICC

        public async void SendICCOnCallExcel(DateTime start, DateTime end, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【InComm卡日報表】  準備進行排程 , 時間 : {now.ToString()}。");
                var @byte1 = await _ICCFactory.GenerateOnCallExcel(start, end);

                _CommonAggregate.Logger.Info($"【InComm卡日報表】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == BatchValue.ICCGroupDaily);
                _CommonAggregate.Logger.Info($"【InComm卡日報表】  取得寄信人清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                var senderList = caseAssigmentGroupList.SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();


                _CommonAggregate.Logger.Info($"【InComm卡日報表】  取得寄信人清單，成功。");

                //加密壓縮Zip
                string fileName = $"InComm卡來電紀錄-S5.xlsx";
                var zipbyte = @byte1.ConvertZipByte(fileName, password);

                #region 組合 payload 寄信
                //主旨
                var mailTitle = $"InComm卡日報表{end.ToString("yyyy/MM/dd")}";

                var reportName1 = $"InComm卡來電紀錄-S5.zip";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;
                //檔案 
                var fileList = new List<HttpFile>();
                fileList.Add(new HttpFile(reportName1, mediaType1, zipbyte));

                CreatMailPayload(mailTitle, senderList, fileList);

                _CommonAggregate.Logger.Info($"【InComm卡日報表】  寄信成功。");


                #endregion

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【InComm卡日報表】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【InComm卡日報表】失敗，原因 : {ex.Message}");
            }
        }

        #endregion


        #region PPCLIFE
        /// <summary>
        /// 統一藥品-品牌商品與問題歸類
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public void SendPPCLIFEBrandCalcExcel(DateTime start, DateTime end, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【統一藥品 - 品牌商品與問題歸類-數據統計報表】  準備進行排程 , 時間 : {now.ToString()}。");
                var @byte = _PPCLIFEFactory.GenerateBrandCalcExcel(start, end);

                _CommonAggregate.Logger.Info($"【統一藥品 - 品牌商品與問題歸類-數據統計報表】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == BatchValue.PPCLIFEBrandGroup);
                _CommonAggregate.Logger.Info($"【統一藥品 - 品牌商品與問題歸類-數據統計報表】  取得寄信人清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                var senderList = caseAssigmentGroupList.SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();
                _CommonAggregate.Logger.Info($"【統一藥品 - 品牌商品與問題歸類-數據統計報表】  取得寄信人清單，成功。");

                #region 組合 payload 寄信
                //主旨
                var mailTitle = $"統一藥品 - 品牌商品與問題歸類-數據統計報表{start.ToString("yyyy/MM")}";

                var reportName1 = $"統一藥品 - 品牌商品與問題歸類-數據統計報表" + end.Year + "-" + end.ToString("MM") + "_(" + start.ToString("MMdd") + "-" + end.ToString("MMdd") + ")" + "-S5.xlsx";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;
                //檔案 
                var fileList = new List<HttpFile>();
                fileList.Add(new HttpFile(reportName1, mediaType1, @byte));

                CreatMailPayload(mailTitle, senderList, fileList);

                _CommonAggregate.Logger.Info($"【統一藥品 - 品牌商品與問題歸類-數據統計報表】  寄信成功。");
                #endregion

            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【統一藥品 - 品牌商品與問題歸類-數據統計報表】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【統一藥品 - 品牌商品與問題歸類-數據統計報表】失敗，原因 : {ex.Message}");
            }
        }
        /// <summary>
        /// 統一藥品來電紀錄 Batch 寄信
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public async void PPCLIFEBatchSendMail(DateTime start, DateTime end, string Type, string batchValue, string password = "")
        {
            try
            {
                var now = DateTime.Now;
                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】  準備進行排程 , 時間 : {now.ToString()}。");

                _CommonAggregate.Logger.Info($"【【統一藥品{Type}報表】  開始取得 統一藥品來電紀錄。");
                //  統一藥品來電紀錄
                var @byte1 = await _PPCLIFEFactory.GenerateOnCallExcelToBatch(start, end);
                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】  開始取得 統一藥品來電紀錄-醫容美學。");//醫容美學
                //統一藥品來電紀錄 - 醫容美學
                var @byte2 = await _PPCLIFEFactory.GenerateAestheticMedicineExcel(start, end);
                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】  開始取得 統一藥品來電紀錄-美麗事業部。");//自有品牌
                // 統一藥品來電紀錄-自有品牌
                var @byte3 = await _PPCLIFEFactory.GenerateOwnBrandExcel(start, end);
                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】  開始取得 統一藥品來電紀錄-保健事業部。");//代理品牌
                // 統一藥品來電紀錄-代理品牌
                var @byte4 = await _PPCLIFEFactory.GenerateProxyBrandExcel(start, end);
                


                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】  準備寄信。");
                //寄信
                var con = new MSSQLCondition<CASE_ASSIGN_GROUP>();
                con.IncludeBy(x => x.CASE_ASSIGN_GROUP_USER);
                con.And(x => x.NAME == batchValue);
                con.Or(x => x.NAME == BatchValue.PPCLIFEMedicineGroup);
                con.Or(x => x.NAME == BatchValue.PPCLIFEOwnBrandGroup);
                con.Or(x => x.NAME == BatchValue.PPCLIFEProxyBrandGroup);
                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】  取得收件者清單。");
                var caseAssigmentGroupList = _CaseAggregate.CaseAssignmentGroup_T1_T2_.GetList(con).ToList();

                #region 收件者
                var senderList1 = caseAssigmentGroupList.Where(c => c.Name == batchValue).SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();
                var senderList2 = caseAssigmentGroupList.Where(c => c.Name == BatchValue.PPCLIFEMedicineGroup).SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();
                var senderList3 = caseAssigmentGroupList.Where(c => c.Name == BatchValue.PPCLIFEOwnBrandGroup).SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();
                var senderList4 = caseAssigmentGroupList.Where(c => c.Name == BatchValue.PPCLIFEProxyBrandGroup).SelectMany(x => x.CaseAssignGroupUsers).Select(x => new ConcatableUser
                {
                    Email = x.Email,
                    UserName = x.UserName,
                    Mobile = x.Telephone,
                    NotificationRemark = x.NotificationRemark
                }).ToList();
                #endregion

                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】  取得收件者清單，成功。");

                #region 組合 payload 寄信
                //主旨
                var mailTitle1 = "統一藥品" + Type + "報表";
                var mailTitle2 = $"統一藥品{Type}報表-醫學美容部";//醫學美容部
                var mailTitle3 = $"統一藥品{Type}報表-美麗事業部";//自有品牌部
                var mailTitle4 = $"統一藥品{Type}報表-保健事業部";//代理品牌部

                if (Type == "日")
                {
                    mailTitle1 += end.ToString("yyyy/MM/dd");
                    mailTitle2 += end.ToString("yyyy/MM/dd");
                    mailTitle3 += end.ToString("yyyy/MM/dd");
                    mailTitle4 += end.ToString("yyyy/MM/dd");
                }
                else if (Type == "月")
                {
                    mailTitle1 += end.ToString("yyyy/MM");
                    mailTitle2 += end.ToString("yyyy/MM");
                    mailTitle3 += end.ToString("yyyy/MM");
                    mailTitle4 += end.ToString("yyyy/MM");
                }

                var reportName1 = $"統一藥品來電紀錄-S5.xlsx";
                var mediaType1 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName1)).MediaType;

                var reportName2 = $"統一藥品來電紀錄-醫學美容部-S5.xlsx";//醫學美容部
                var mediaType2 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName2)).MediaType;
                var reportName3 = $"統一藥品來電紀錄-美麗事業部-S5.xlsx";//自有品牌部
                var mediaType3 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName3)).MediaType;
                var reportName4 = $"統一藥品來電紀錄-保健事業部-S5.xlsx";//代理品牌部
                var mediaType4 = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(reportName4)).MediaType;
                //檔案 
                var fileList1 = new List<HttpFile>();
                var fileList2 = new List<HttpFile>();
                var fileList3 = new List<HttpFile>();
                var fileList4 = new List<HttpFile>();

                fileList1.Add(new HttpFile(reportName1, mediaType1, @byte1));
                fileList2.Add(new HttpFile(reportName2, mediaType2, @byte2));
                fileList3.Add(new HttpFile(reportName3, mediaType3, @byte3));
                fileList4.Add(new HttpFile(reportName4, mediaType4, @byte4));

                CreatMailPayload(mailTitle1, senderList1, fileList1);
                CreatMailPayload(mailTitle2, senderList2, fileList2);
                CreatMailPayload(mailTitle3, senderList3, fileList3);
                CreatMailPayload(mailTitle4, senderList4, fileList4);

                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】  寄信成功。");
                #endregion


            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Info($"【統一藥品{Type}報表】失敗，原因 : " + ex.ToString());
                _CommonAggregate.Loggers["Email"].Error($"【統一藥品{Type}報表】失敗，原因 : {ex.Message}");
            }

        }
        #endregion


        #endregion

        /// <summary>
        /// 組合收件者，寄信
        /// </summary>
        /// <param name="mailTitle"></param>
        /// <param name="senderList"></param>
        /// <param name="fileList"></param>
        private void CreatMailPayload(string mailTitle, List<ConcatableUser> senderList, List<HttpFile> fileList)
        {
            var emailPayload = new EmailPayload()
            {
                Title = mailTitle,
                Sender = new ConcatableUser() { Email = GlobalizationCache.Instance.AdminMailAddress,UserName = GlobalizationCache.APName },
                Receiver = senderList.Where(x => x.NotificationRemark == ((int)EmailReceiveType.Recipient).ToString()).ToList(),
                Cc = senderList.Where(x => x.NotificationRemark == ((int)EmailReceiveType.CC).ToString()).ToList(),
                Bcc = senderList.Where(x => x.NotificationRemark == ((int)EmailReceiveType.BCC).ToString()).ToList(),
                Attachments = fileList
            };

            // 附件給系統管理員
            var sysCC = new ConcatableUser()
            {
                Email = GlobalizationCache.Instance.AdminMailReceiverAddress
            };

            emailPayload.Cc.Add(sysCC);

            _NotificationProviders[NotificationType.Email].Send(
                    payload: emailPayload
                );

        }


        private void SendCaseRemindNotificationSuccess(List<int> caseRemindIDs)
        {
            var con = new MSSQLCondition<CASE_REMIND>();

            con.ActionModify(x => x.IS_NOTIFCATED = true);

            caseRemindIDs.ForEach(id => con.Or(g => g.ID == id));

            _CaseAggregate.CaseRemind_T1_T2_.UpdateRange(con);
        }
    }
}
