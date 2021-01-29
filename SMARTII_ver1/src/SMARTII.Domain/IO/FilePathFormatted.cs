using System;
using System.Collections.Generic;
using System.IO;

namespace SMARTII.Domain.IO
{
    public static class FilePathFormatted
    {
        public static string ItemImageVirtualPath = "File/GetItemImage?buID={0}&ID={1}&fileName={2}";
        public static string UserImageVirtualPath = "File/GetUserImage?fileName={0}";
        public static string EmailVirtualPath = "File/GetEmail?fileName={0}";
        public static string BillboardVirtualPath = "File/GetBillboard?createDate={0}&ID={1}&fileName={2}";
        public static string KMVirtualPath = "File/GetKM?buID={0}&classificationID={1}&dataID={2}&fileName={3}";
        public static string CaseVirtualPath = "File/GetCase?buID={0}&createDate={1}&caseID={2}&fileName={3}";

        public static string CaseAssignmentVirtualPath = "File/GetCaseAssignment?buID={0}&createDate={1}&caseID={2}&caseAssignID={3}&fileName={4}";
        public static string CaseComplaintInvoiceVirtualPath = "File/GetCaseComplaintInvoice?buID={0}&createDate={1}&caseID={2}&invoiceID={3}&fileName={4}";
        public static string CaseComplaintNoticeVirtualPath = "File/GetCaseComplaintNotice?buID={0}&createDate={1}&caseID={2}&noticeID={3}&fileName={4}";

        public static string OfficialEmailVirtualPath = "File/GetOfficialEmail?buID={0}&mesgID={1}&fileName={2}";

        /// <summary>
        /// [寄出]信件寄送保留暫存目錄
        /// </summary>
        public static string EmailTempDirPath(string random)
        {
            return $@"{Path.GetTempPath()}\{random}"; 
        }

        /// <summary>
        /// [寄出]信件存放檔案路徑
        /// </summary>
        public static string EmailSenderSaverDirPath = $@"C:\SMARTII\Email\";

        /// <summary>
        /// [寄出]信件存放檔案路徑
        /// 0 : 檔案名稱
        /// </summary>
        public static string EmailSenderPhysicalFilePath = @"C:\SMARTII\Email\{0}";

        /// <summary>
        /// 公佈欄檔案存放目錄
        /// 0 : 建立日期 (yyyyMMdd)
        /// 1 : 公佈欄編號
        /// </summary>
        public static string BillboardSaverDirPath = @"C:\SMARTII\Billboard\{0}\{1}";

        /// <summary>
        /// 公佈欄檔案存放路徑
        /// 0 : 建立日期 (yyyyMMdd)
        /// 1 : 公佈欄編號
        /// 2 : 檔案名稱
        /// </summary>
        public static string BillboardPhysicalFilePath = @"C:\SMARTII\Billboard\{0}\{1}\{2}";

        /// <summary>
        /// 商品檔案存放目錄
        /// 0 : 建立日期 (yyyyMMdd)
        /// 1 : 公佈欄編號
        /// </summary>
        public static string ItemSaverDirPath = @"C:\SMARTII\Item\{0}\{1}";

        /// <summary>
        /// 商品檔案存放路徑
        /// 0 : 建立日期 (yyyyMMdd)
        /// 1 : 公佈欄編號
        /// 2 : 檔案名稱
        /// </summary>
        public static string ItemPhysicalFilePath = @"C:\SMARTII\Item\{0}\{1}\{2}";

        /// <summary>
        /// 常見問題分類檔案存放目錄
        /// 0 : 企業別代號
        /// 1 : KM分類代號
        /// 2 : KM細項(資料代號)
        /// </summary>
        public static string KMSaverDirPath = @"C:\SMARTII\KM\{0}\{1}\{2}";

        /// <summary>
        /// 常見問題分類檔案存放路徑
        /// 0 : 企業別代號
        /// 1 : KM分類代號
        /// 2 : KM細項(資料代號)
        /// 3 : 檔案名稱
        /// </summary>
        public static string KMPhysicalFilePath = @"C:\SMARTII\KM\{0}\{1}\{2}\{3}";

        /// <summary>
        /// 案件附件存放目錄
        /// 0 : 企業別代號
        /// 1 : 建立日期 (yyyyMMdd)
        /// 2 : 案件編號
        /// </summary>
        public static string CaseSaverDirPath = @"C:\SMARTII\Case\{0}\{1}\{2}";

        /// <summary>
        /// 案件附件存放路徑
        /// 0 : 企業別代號
        /// 1 : 建立日期 (yyyyMMdd)
        /// 2 : 案件編號
        /// 3 : 檔案名稱
        /// </summary>
        public static string CasePhysicalFilePath = @"C:\SMARTII\Case\{0}\{1}\{2}\{3}";

        /// <summary>
        /// 轉派附件存放目錄
        /// 0 : 企業別代號
        /// 1 : 建立日期 (yyyyMMdd)
        /// 2 : 案件編號
        /// 3 : 轉派序號
        /// </summary>
        public static string CaseAssignmentSaverDirPath = @"C:\SMARTII\Case\{0}\{1}\{2}\轉派\{3}";

        /// <summary>
        /// 轉派附件存放路徑
        /// 0 : 企業別代號
        /// 1 : 建立日期 (yyyyMMdd)
        /// 2 : 案件編號
        /// 3 : 轉派序號
        /// 4 : 檔案名稱
        /// </summary>
        public static string CaseAssignmentPhysicalFilePath = @"C:\SMARTII\Case\{0}\{1}\{2}\轉派\{3}\{4}";


        /// <summary>
        /// 一般通知附件存放目錄
        /// 0 : 企業別代號
        /// 1 : 建立日期 (yyyyMMdd)
        /// 2 : 案件編號
        /// 3 : 通知序號
        /// </summary>
        public static string CaseComplaintNoticeSaverDirPath = @"C:\SMARTII\Case\{0}\{1}\{2}\一般通知\{3}";

        /// <summary>
        /// 一般通知附件存放路徑
        /// 0 : 企業別代號
        /// 1 : 建立日期 (yyyyMMdd)
        /// 2 : 案件編號
        /// 3 : 通知序號
        /// 4 : 檔案名稱
        /// </summary>
        public static string CaseComplaintNoticePhysicalFilePath = @"C:\SMARTII\Case\{0}\{1}\{2}\一般通知\{3}\{4}";

        /// <summary>
        /// 反應單附件存放目錄
        /// 0 : 企業別代號
        /// 1 : 建立日期 (yyyyMMdd)
        /// 2 : 案件編號
        /// 3 : 反應單單號
        /// </summary>
        public static string CaseComplaintInvoiceSaverDirPath = @"C:\SMARTII\Case\{0}\{1}\{2}\反應單\{3}";

        /// <summary>
        /// 反應單附件存放路徑
        /// 0 : 企業別代號
        /// 1 : 建立日期 (yyyyMMdd)
        /// 2 : 案件編號
        /// 3 : 反應單單號
        /// 4 : 檔案名稱
        /// </summary>
        public static string CaseComplaintInvoicePhysicalFilePath = @"C:\SMARTII\Case\{0}\{1}\{2}\反應單\{3}\{4}";


        /// <summary>
        /// 官網來信 Email
        /// 0 : 企業別代號
        /// 1 : yyyyMMddhhmmssfff
        /// </summary>
        public static string OfficialEmailPhysicalDirPath = @"C:\SMARTII\BuEmail\{0}\{1}";


        /// <summary>
        /// 官網來信 Email
        /// 0 : 企業別代號
        /// 1 : yyyyMMddhhmmssfff
        /// 2 : 檔案名稱
        /// </summary>
        public static string OfficialEmailPhysicalFilePath = @"C:\SMARTII\BuEmail\{0}\{1}\{2}";
        
        /// <summary>
        /// 使用者 大頭照
        /// 0 : 企業別代號
        /// </summary>
        public static string UserImagePhysicalFilePath = @"C:\SMARTII\User\{0}";


        public static string ParseFileName(this string saverPath)
        {
            var array = saverPath?.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries);

            if (array == null || array.Length == 0) return string.Empty;

            return array[array.Length - 1];
        }

        public static string GetEmailSenderPhysicalDirPath()
        {
            return EmailSenderSaverDirPath;
        }

        public static string GetEmailSenderPhysicalFilePath(string fileName)
        {
            return string.Format(EmailSenderPhysicalFilePath, fileName);
        }

        public static string GetEmailSenderVirtualFilePath(string fileName)
        {
            return string.Format(EmailVirtualPath, fileName);
        }

        public static string GetBillboardPhysicalFilePath(string createDate, int ID, string fileName)
        {
            return string.Format(BillboardPhysicalFilePath, createDate, ID.ToString(), fileName);
        }

        public static string GetBillboardPhysicalDirPath(string createDate, int ID)
        {
            return string.Format(BillboardSaverDirPath, createDate, ID.ToString());
        }

        public static string GetBillboardVirtualFilePath(string createDate, int ID, string fileName)
        {
            return string.Format(BillboardVirtualPath, createDate, ID.ToString(), fileName);
        }

        public static string GetItemPhysicalFilePath(int buID, int ID, string fileName)
        {
            return string.Format(ItemPhysicalFilePath, buID.ToString(), ID.ToString(), fileName);
        }

        public static string GetItemPhysicalDirPath(int buID, int ID)
        {
            return string.Format(ItemSaverDirPath, buID.ToString(), ID.ToString());
        }

        public static string GetItemVirtualFilePath(int buID, int ID, string fileName)
        {
            return string.Format(ItemImageVirtualPath, buID.ToString(), ID.ToString(), fileName);
        }

        public static string GetUserImageVirtualPath(string fileName)
        {
            return string.Format(UserImageVirtualPath, fileName);
        }

        public static string GetUserImagePhysicalPath(string fileName)
        {
            return string.Format(UserImagePhysicalFilePath, fileName);
        }



        public static string GetKMPhysicalFilePath(int buID, int classificationID, int dataID, string fileName)
        {
            return string.Format(KMPhysicalFilePath, buID.ToString(), classificationID.ToString(), dataID.ToString(), fileName);
        }

        public static string GetKMPhysicalDirPath(int buID, int classificationID, int dataID)
        {
            return string.Format(KMSaverDirPath, buID.ToString(), classificationID.ToString(), dataID.ToString());
        }

        public static string GetKMVirtualFilePath(int buID, int classificationID, int dataID, string fileName)
        {
            return string.Format(KMVirtualPath, buID.ToString(), classificationID.ToString(), dataID.ToString(), fileName);
        }


        public static string GetCasePhysicalFilePath(int buID, string createDate, string caseID, string fileName)
        {
            return string.Format(CasePhysicalFilePath, buID.ToString(), createDate, caseID, fileName);
        }

        public static string GetCasePhysicalDirPath(int buID, string createDate, string caseID)
        {
            return string.Format(CaseSaverDirPath, buID.ToString(), createDate, caseID);
        }

        public static string GetCaseVirtualFilePath(int buID, string createDate, string caseID, string fileName)
        {
            return string.Format(CaseVirtualPath, buID, createDate, caseID, fileName);
        }


        public static string GetCaseAssignmentPhysicalFilePath(int buID, string createDate, string caseID, int assignmentID, string fileName)
        {
            return string.Format(CaseAssignmentPhysicalFilePath, buID.ToString(), createDate, caseID, assignmentID.ToString(), fileName);
        }
        public static string GetCaseAssignmentPhysicalDirPath(int buID, string createDate, string caseID, int assignmentID)
        {
            return string.Format(CaseAssignmentSaverDirPath, buID.ToString(), createDate, caseID, assignmentID.ToString());
        }
        public static string GetCaseAssignmentVirtualFilePath(int buID, string createDate, string caseID, int assignmentID, string fileName)
        {
            return string.Format(CaseAssignmentVirtualPath, buID, createDate, caseID, assignmentID.ToString(), fileName);
        }


        public static string GetCaseComplaintInvoicePhysicalFilePath(int buID, string createDate, string caseID, string invoiceID, string fileName)
        {
            return string.Format(CaseComplaintInvoicePhysicalFilePath, buID.ToString(), createDate, caseID, invoiceID, fileName);
        }
        public static string GetCaseComplaintInvoicePhysicalDirPath(int buID, string createDate, string caseID, string invoiceID)
        {
            return string.Format(CaseComplaintInvoiceSaverDirPath, buID.ToString(), createDate, caseID, invoiceID);
        }

        public static string GetCaseComplaintInvoiceVirtualFilePath(int buID, string createDate, string caseID, string invoiceID, string fileName)
        {
            return string.Format(CaseComplaintInvoiceVirtualPath, buID, createDate, caseID, invoiceID, fileName);
        }


        public static string GetCaseComplaintNoticePhysicalFilePath(int buID, string createDate, string caseID, int noticeID, string fileName)
        {
            return string.Format(CaseComplaintNoticePhysicalFilePath, buID.ToString(), createDate, caseID, noticeID.ToString(), fileName);
        }

        public static string GetCaseComplaintNoticePhysicalDirPath(int buID, string createDate, string caseID, int noticeID)
        {
            return string.Format(CaseComplaintNoticeSaverDirPath, buID.ToString(), createDate, caseID, noticeID.ToString());
        }

        public static string GetCaseComplaintNoticeVirtualFilePath(int buID, string createDate, string caseID, int noticeID, string fileName)
        {
            return string.Format(CaseComplaintNoticeVirtualPath, buID, createDate, caseID, noticeID.ToString(), fileName);
        }
        public static string GetOfficialEmailPhysicalDirPath(int buID, string guid)
        {
            return string.Format(OfficialEmailPhysicalDirPath, buID, guid);
        }
        public static string GetOfficialEmailVirtualFilePath(int buID, string guid, string fileName)
        {
            return string.Format(OfficialEmailVirtualPath, buID, guid, fileName);
        }

        public static string GetVirtualFilePath(string Path)
        {
            var strList = ParsinRequestParam(Path);
            if (Path.Contains("File/GetCase"))
            {
                return GetCasePhysicalFilePath(int.Parse(strList[0]), strList[1], strList[2], strList[3]);
            }
            else if (Path.Contains("File/GetOfficialEmail"))
            {
                return string.Format(OfficialEmailPhysicalFilePath, strList[0], strList[1], strList[2]);
            }

            return "";
        }

        public static List<string> ParsinRequestParam(string req)
        {
            var result = new List<string>();

            if (!string.IsNullOrEmpty(req))
            {

                var _temp = req;
                var tag = string.Empty;

                // 取得 Keyword poisition
                var left_index = _temp.IndexOf('=');
                var right_index = _temp.IndexOf('&');


                // 表示最後一筆 request 參數
                if (right_index < 0)
                {
                    tag = _temp.Substring(left_index);
                    result.Add(tag.Replace("=", "").Replace("&", ""));
                    return result;
                }
                else
                {
                    // 計算 擷取字串長度
                    var offset = (right_index - left_index) + 1;

                    tag = _temp.Substring(left_index, offset);
                    result.Add(tag.Replace("=", "").Replace("&", ""));
                }



                // 清掉這次的 Tag 文字
                var subTemp = _temp.Substring(right_index + 1);

                result.AddRange(ParsinRequestParam(subTemp));

            }


            return result;
        }


    }

}
