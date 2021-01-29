using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using MultipartDataMediaFormatter.Infrastructure;
using SMARTII.Domain.Case;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;

namespace SMARTII.Domain.IO
{
    public static class FileSaverUtility
    {
        /// <summary>
        /// 儲存案件附件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="case"></param>
        /// <param name="files"></param>
        /// <returns></returns>
        public static List<string> SaveCaseFiles(FileProcessContext context, Case.Case @case, List<HttpFile> files)
        {
            var pathArray = new List<string>();

            files?.ForEach(file =>
            {
                var virtualPath = FilePathFormatted.GetCaseVirtualFilePath(@case.NodeID, @case.CreateDateTime.ToString("yyyyMMdd"), @case.CaseID, file.FileName.Trim());
                var physicalDirPath = FilePathFormatted.GetCasePhysicalDirPath(@case.NodeID, @case.CreateDateTime.ToString("yyyyMMdd"), @case.CaseID);

                var path = file.Buffer.SaveAsFilePath(physicalDirPath, file.FileName.Trim());

                context.Paths.Add(path);
                pathArray.Add(virtualPath);
            });

            return pathArray;
        }

        /// <summary>
        /// 儲存一般通知附件
        /// </summary>
        /// <param name="notice"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<string> SaveAssignmentNoticeFiles(FileProcessContext context, CaseAssignmentComplaintNotice notice)
        {
            var pathArray = new List<string>();

            notice?.Files?.ForEach(file =>
            {
                var virtualPath = FilePathFormatted.GetCaseComplaintNoticeVirtualFilePath(
                    notice.NodeID, notice.CreateDateTime.ToString("yyyyMMdd"), notice.CaseID, notice.ID, file.FileName.Trim());

                var physicalDirPath = FilePathFormatted.GetCaseComplaintNoticePhysicalDirPath(
                    notice.NodeID, notice.CreateDateTime.ToString("yyyyMMdd"), notice.CaseID, notice.ID);

                var path = file.Buffer.SaveAsFilePath(physicalDirPath, file.FileName.Trim());

                context.Paths.Add(path);
                pathArray.Add(virtualPath);
            });

            return pathArray;
        }

        /// <summary>
        /// 儲存公佈欄附件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<string> SaveBillboardFiles(FileProcessContext context, Billboard data)
        {
            var pathArray = new List<string>();

            data?.Files?.ForEach(file =>
            {
                var virtualPath = FilePathFormatted.GetBillboardVirtualFilePath(data.CreateDateTime.ToString("yyyyMMdd"), data.ID, file.FileName.Trim());
                var physicalDirPath = FilePathFormatted.GetBillboardPhysicalDirPath(data.CreateDateTime.ToString("yyyyMMdd"), data.ID);

                var path = file.Buffer.SaveAsFilePath(physicalDirPath, file.FileName.Trim());

                context.Paths.Add(path);
                pathArray.Add(virtualPath);
            });

            return pathArray;
        }

        /// <summary>
        /// 儲存常用問題討論區附件
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <param name="classification"></param>
        /// <returns></returns>
        public static List<string> SaveKMFiles(FileProcessContext context, KMData data, KMClassification classification)
        {
            var pathArray = new List<string>();

            data?.Files?.ForEach(file =>
            {
                var virtualPath = FilePathFormatted.GetKMVirtualFilePath(classification.NodeID, classification.ID, data.ID, file.FileName.Trim());
                var physicalDirPath = FilePathFormatted.GetKMPhysicalDirPath(classification.NodeID, classification.ID, data.ID);

                var path = file.Buffer.SaveAsFilePath(physicalDirPath, file.FileName.Trim());

                context.Paths.Add(path);
                pathArray.Add(virtualPath);
            });

            return pathArray;
        }

        /// <summary>
        /// 儲存使用者設定附件
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<string> SaveUserParameterFile(FileProcessContext context, UserParameter data)
        {
            var pathArray = new List<string>();

            //組成相對路徑
            var virtualPath = FilePathFormatted.GetUserImageVirtualPath(data.UserID + "." + data.Picture.FileName.Trim().Split('.').Last());

            var physicalDirPath = FilePathFormatted.GetUserImagePhysicalPath(data.UserID + "." + data.Picture.FileName.Trim().Split('.').Last());

            var path = data.Picture.Buffer.SaveAsFilePath(physicalDirPath);

            data.ImagePath = virtualPath;
            context.Paths.Add(path);
            pathArray.Add(virtualPath);

            return pathArray;
        }

        /// <summary>
        /// 儲存商品圖片
        /// </summary>
        /// <param name="context"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<string> SaveItemFiles(FileProcessContext context, Item data)
        {
            var pathArray = new List<string>();

            data?.Picture?.ForEach(file =>
            {
                var virtualPath = FilePathFormatted.GetItemVirtualFilePath(data.NodeID, data.ID, file.FileName.Trim());
                var physicalDirPath = FilePathFormatted.GetItemPhysicalDirPath(data.NodeID, data.ID);

                var path = file.Buffer.SaveAsFilePath(physicalDirPath, file.FileName.Trim());

                context.Paths.Add(path);
                pathArray.Add(virtualPath);
            });

            return pathArray;
        }

        /// <summary>
        /// 儲存反應單附件
        /// </summary>
        /// <param name="invoice"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<string> SaveAssignmentInvoiceFiles(FileProcessContext context, CaseAssignmentComplaintInvoice invoice)
        {
            var pathArray = new List<string>();

            invoice?.Files?.ForEach(file =>
            {
                var virtualPath = FilePathFormatted.GetCaseComplaintInvoiceVirtualFilePath(
                    invoice.NodeID, invoice.CreateDateTime.ToString("yyyyMMdd"), invoice.CaseID, invoice.InvoiceID, file.FileName.Trim());

                var physicalDirPath = FilePathFormatted.GetCaseComplaintInvoicePhysicalDirPath(
                    invoice.NodeID, invoice.CreateDateTime.ToString("yyyyMMdd"), invoice.CaseID, invoice.InvoiceID);

                var path = file.Buffer.SaveAsFilePath(physicalDirPath, file.FileName.Trim());

                context.Paths.Add(path);
                pathArray.Add(virtualPath);
            });

            return pathArray;
        }

        /// <summary>
        /// 儲存轉派附件
        /// </summary>
        /// <param name="assign"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<string> SaveCaseAssignmentFiles(FileProcessContext context, CaseAssignment assign, List<HttpFile> files)
        {
            var pathArray = new List<string>();

            files?.ForEach(file =>
            {
                var virtualPath = FilePathFormatted.GetCaseAssignmentVirtualFilePath(
                    assign.NodeID, assign.CreateDateTime.ToString("yyyyMMdd"), assign.CaseID, assign.AssignmentID, file.FileName.Trim());

                var physicalDirPath = FilePathFormatted.GetCaseAssignmentPhysicalDirPath(
                    assign.NodeID, assign.CreateDateTime.ToString("yyyyMMdd"), assign.CaseID, assign.AssignmentID);

                var path = file.Buffer.SaveAsFilePath(physicalDirPath, file.FileName.Trim());

                context.Paths.Add(path);
                pathArray.Add(virtualPath);
            });

            return pathArray;
        }

        public static string SaveMailFiles(FileProcessContext context, OfficialEmailEffectivePayload data)
        {
            string path = "";

            var guid = DateTime.Today.ToString("yyyyMMdd")+Guid.NewGuid().ToString();

            var virtualPath = FilePathFormatted.GetOfficialEmailVirtualFilePath(data.NodeID, guid, data.Mail.FileName.Trim());
            var physicalDirPath = FilePathFormatted.GetOfficialEmailPhysicalDirPath(data.NodeID, guid);

            path = data.Mail.Buffer.SaveAsFilePath(physicalDirPath, data.Mail.FileName.Trim());

            context.Paths.Add(path);
            path = virtualPath;

            return path;
        }

        /// <summary>
        /// 驗證傳入參數是否符合MesgID規則
        /// </summary>
        /// <param name="mesgID"></param>
        /// <returns></returns>
        public static string IsMesgID(string mesgID)
        {
            try
            {
                //8碼後字串為Guid
                Guid mesgGuid = new Guid(mesgID.Substring(8));
                //前8碼為日期
                var mesgDate = DateTime.ParseExact(mesgID.Substring(0, 8), "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces);
                return mesgID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
