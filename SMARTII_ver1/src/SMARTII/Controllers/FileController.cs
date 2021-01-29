using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Autofac.Features.Indexed;
using SMARTII.Assist.Logger;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Case;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Models.File;
using SMARTII.Resource.Tag;

namespace SMARTII.Controllers
{
    [AllowAnonymous]
    public class FileController : ApiController
    {
        private readonly IUserFacade _UserFacade;
        private readonly ICaseFacade _CaseFacade;
        private readonly IItemFactory _ItemFactory;
        private readonly ICommonAggregate _CommonAggregate;
        private readonly IBillboardFacade _BillboardFacade;
        private readonly ICaseAssignmentFacade _CaseAssignmentFacade;
        private readonly IKMClassificationFacade _KMClassificationFacade;



        public FileController(IUserFacade UserFacade,
                              ICaseFacade CaseFacade,
                              ICommonAggregate CommonAggregate,
                              IBillboardFacade BillboardFacade,
                              ICaseAssignmentFacade CaseAssignmentFacade,
                              IIndex<string, IItemFactory> ItemFactories,
                              IKMClassificationFacade KMClassificationFacade)
        {
            _UserFacade = UserFacade;
            _CaseFacade = CaseFacade;
            _CommonAggregate = CommonAggregate;
            _BillboardFacade = BillboardFacade;
            _CaseAssignmentFacade = CaseAssignmentFacade;
            _KMClassificationFacade = KMClassificationFacade;
            _ItemFactory = ItemFactories[EssentialCache.BusinessKeyValue.COMMONBU];
        }

        /// <summary>
        /// 取得寄出的信件內容
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetEmail(string fileName)
        {
            try
            {
                if (Path.IsPathRooted(fileName))
                {
                    throw new ArgumentNullException("error:信件名為絕對路徑");
                }
                string path = Path.Combine(FilePathFormatted.EmailSenderSaverDirPath, Path.GetFileName(fileName));
                var imgByte = File.ReadAllBytes(path);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(imgByte)
                };
                resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("message/rfc822");
                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 取得使用者圖片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetUserImage(string fileName)
        {
            try
            {
                //string path = $@"C:\新增資料夾\{fileName}";
                string path = FilePathFormatted.GetUserImagePhysicalPath(Path.GetFileName(fileName));

                var imgByte = File.ReadAllBytes(path);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(imgByte)
                };
                resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));
                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 取得商品圖片
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetItemImage(int buID, int ID, string fileName)
        {
            //try
            //{
            //    string path = $@"C:\新增資料夾\{fileName}";
            //    var imgByte = File.ReadAllBytes(path);
            //    var resp = new HttpResponseMessage(HttpStatusCode.OK)
            //    {
            //        Content = new ByteArrayContent(imgByte)
            //    };
            //    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("image/jpg");
            //    return resp;
            //}
            //catch (Exception)
            //{
            //    //_CommonAggregate.Logger.Error(ex);

            //    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            //}
            try
            {
                var path = FilePathFormatted.GetItemPhysicalFilePath(buID, ID, Path.GetFileName(fileName));

                FileInfo fileInfo = new FileInfo(path);

                using (FileStream fs = fileInfo.OpenRead())
                {
                    byte[] data = fs.StreamToBytes();

                    var resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(data)
                    };
                    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileInfo.Name
                    };
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 取得公佈欄
        /// </summary>
        /// <param name="createDateTime"> 建立年月日</param>
        /// <param name="ID">公佈欄代號</param>
        /// <param name="fileName">檔案名稱</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetBillboard(string createDate, int ID, string fileName)
        {
            try
            {
                var path = FilePathFormatted.GetBillboardPhysicalFilePath(createDate, ID, fileName);

                FileInfo fileInfo = new FileInfo(path);

                using (FileStream fs = fileInfo.OpenRead())
                {
                    byte[] data = fs.StreamToBytes();

                    var resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(data)
                    };
                    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileInfo.Name
                    };
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 取得常見問題討論區附件
        /// </summary>
        /// <param name="createDateTime"> 建立年月日</param>
        /// <param name="ID">公佈欄代號</param>
        /// <param name="fileName">檔案名稱</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetKM(int buID, int classificationID, int dataID, string fileName)
        {
            try
            {
                var path = FilePathFormatted.GetKMPhysicalFilePath(buID, classificationID, dataID, fileName);

                FileInfo fileInfo = new FileInfo(path);

                using (FileStream fs = fileInfo.OpenRead())
                {
                    byte[] data = fs.StreamToBytes();

                    var resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(data)
                    };
                    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileInfo.Name
                    };
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 取得案件內容/結案內容附件
        /// </summary>
        /// <param name="buID">企業別代號</param>
        /// <param name="createDate">建立日期</param>
        /// <param name="caseID">案件編號</param>
        /// <param name="fileName">檔案名稱</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetCase(int buID, string createDate, string caseID, string fileName)
        {
            try
            {
                var path = FilePathFormatted.GetCasePhysicalFilePath(buID, createDate, caseID, fileName);

                FileInfo fileInfo = new FileInfo(path);

                using (FileStream fs = fileInfo.OpenRead())
                {
                    byte[] data = fs.StreamToBytes();

                    var resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(data)
                    };
                    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileInfo.Name
                    };
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 取得案件一般通知附件
        /// </summary>
        /// <param name="buID"></param>
        /// <param name="createDate"></param>
        /// <param name="caseID"></param>
        /// <param name="noticeID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetCaseComplaintNotice(int buID, string createDate, string caseID, int noticeID, string fileName)
        {
            try
            {
                var path = FilePathFormatted.GetCaseComplaintNoticePhysicalFilePath(buID, createDate, caseID, noticeID, fileName);

                FileInfo fileInfo = new FileInfo(path);

                using (FileStream fs = fileInfo.OpenRead())
                {
                    byte[] data = fs.StreamToBytes();

                    var resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(data)
                    };
                    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileInfo.Name
                    };
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 取得案件一般通知附件
        /// </summary>
        /// <param name="buID"></param>
        /// <param name="createDate"></param>
        /// <param name="caseID"></param>
        /// <param name="noticeID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetCaseComplaintInvoice(int buID, string createDate, string caseID, string invoiceID, string fileName)
        {
            try
            {
                var path = FilePathFormatted.GetCaseComplaintInvoicePhysicalFilePath(buID, createDate, caseID, invoiceID, fileName);

                FileInfo fileInfo = new FileInfo(path);

                using (FileStream fs = fileInfo.OpenRead())
                {
                    byte[] data = fs.StreamToBytes();

                    var resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(data)
                    };
                    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileInfo.Name
                    };
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// 取得轉派附件
        /// </summary>
        /// <param name="buID"></param>
        /// <param name="createDate"></param>
        /// <param name="caseID"></param>
        /// <param name="caseAssignID"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetCaseAssignment(int buID, string createDate, string caseID, int caseAssignID, string fileName)
        {
            try
            {
                var path = FilePathFormatted.GetCaseAssignmentPhysicalFilePath(buID, createDate, caseID, caseAssignID, fileName);

                FileInfo fileInfo = new FileInfo(path);

                using (FileStream fs = fileInfo.OpenRead())
                {
                    byte[] data = fs.StreamToBytes();

                    var resp = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ByteArrayContent(data)
                    };
                    resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                    {
                        FileName = fileInfo.Name
                    };
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeMapping.GetMimeMapping(fileName));

                    return resp;
                }
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        /// <summary>
        /// 刪除商品圖片
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Item_lang.ITEM_DELETE_IMAGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteItemImage(Models.File.FileViewModel model)
        {
            try
            {
                var itemID = int.Parse(model.id);

                var fileName = model.key.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                _ItemFactory.DeleteImage(itemID, fileName);

                return await new JsonResult(Item_lang.ITEM_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(Item_lang.ITEM_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(Item_lang.ITEM_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 刪除使用者圖片
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(User_lang.USER_DELETE_IMAGE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteUserImage(FileViewModel model)
        {
            try
            {
                var fileName = model.key.Split('=')[1];

                _UserFacade.DeleteImage(model.id, fileName);

                return await new JsonResult(User_lang.USER_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 刪除公佈欄附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE_FILE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteBillboardFile(FileViewModel model)
        {
            try
            {
                var bullboardID = int.Parse(model.id);

                var fileName = model.key.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                _BillboardFacade.DeleteFileWithUpdate(bullboardID, fileName);

                return await new JsonResult(User_lang.USER_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 刪除常見問題討論區附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE_FILE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteKMFile(FileViewModel model)
        {
            try
            {
                var bullboardID = int.Parse(model.id);

                var fileName = model.key.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                _KMClassificationFacade.DeleteFileWithUpdate(bullboardID, fileName);

                return await new JsonResult(User_lang.USER_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 刪除案件附件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE_FILE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteCaseFile(FileViewModel model)
        {
            try
            {
                var caseID = model.id;

                var fileName = model.key.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                _CaseFacade.DeleteFileWithUpdate(caseID, fileName, CaseType.Filling);

                return await new JsonResult(User_lang.USER_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 刪除結案附件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE_FILE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteCaseFinishFile(FileViewModel model)
        {
            try
            {
                var caseID = model.id;

                var fileName = model.key.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                _CaseFacade.DeleteFileWithUpdate(caseID, fileName, CaseType.Finished);

                return await new JsonResult(User_lang.USER_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 刪除一般通知附件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE_FILE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteCaseAssignmentNoticeFile(FileViewModel model)
        {
            try
            {

                var noticeID = int.Parse(model.id);

                var fileName = model.key.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                _CaseAssignmentFacade.DeleteNoticeFileWithUpdate(noticeID, fileName);

                return await new JsonResult(User_lang.USER_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }

        /// <summary>
        /// 刪除反應單附件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE_FILE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteCaseAssignmentInvocieFile(FileViewModel model)
        {
            try
            {
                var identityID = int.Parse(model.id);

                var fileName = model.key.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                _CaseAssignmentFacade.DeleteInvoiceFileWithUpdate(identityID, fileName);

                return await new JsonResult(User_lang.USER_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }


        /// <summary>
        /// 刪除案件附件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Logger(nameof(Billboard_lang.BILLBOARD_DELETE_FILE))]
        [ModelValidator(false)]
        public async Task<IHttpResult> DeleteCaseAssignmentFile(FileViewModel model)
        {
            try
            {
                var caseID = model.extend.ToString();
                var assignmentID = int.Parse(model.id);

                var fileName = model.key.Split(new string[] { "fileName=" }, StringSplitOptions.RemoveEmptyEntries)[1];

                _CaseAssignmentFacade.DeleteAssignmentFinishFileWithUpdate(caseID, assignmentID, fileName);

                return await new JsonResult(User_lang.USER_DELETE_IMAGE_SUCCESS, true).Async();
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(
                 ex.PrefixDevMessage(User_lang.USER_DELETE_IMAGE_FAIL));

                return await new JsonResult(
                    ex.PrefixMessage(User_lang.USER_DELETE_IMAGE_FAIL), false)
                    .Async();
            }
        }


        /// <summary>
        /// 取得企業別共用信箱信件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<HttpResponseMessage> GetOfficialEmail(int buID, string mesgID, string fileName)
        {
            try
            {
                string path = string.Format(FilePathFormatted.OfficialEmailPhysicalFilePath, buID.ToString(), FileSaverUtility.IsMesgID(mesgID), Path.GetFileName(fileName));
                var imgByte = File.ReadAllBytes(path);
                var resp = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(imgByte)
                };
                resp.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("message/rfc822");
                return resp;
            }
            catch (Exception ex)
            {
                _CommonAggregate.Logger.Error(ex);

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}
