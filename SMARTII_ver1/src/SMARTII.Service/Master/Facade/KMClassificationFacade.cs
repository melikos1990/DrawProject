using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using MoreLinq;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.System;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class KMClassificationFacade : IKMClassificationFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public KMClassificationFacade(IMasterAggregate MasterAggregate,
                                      IOrganizationAggregate OrganizationAggregate
                                          )
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }



        /// <summary>
        /// 單一新增明細
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Create(KMData data)
        {
            //驗證主旨是否重複
            var isExistName = _MasterAggregate.KMData_T1_
                                                 .HasAny(x => x.CLASSIFICATION_ID == data.ID &&
                                                              x.TITLE == data.Title);
            if (isExistName)
                throw new Exception(KMClassification_lang.KMCLASSIFICATION_DUPLICATE_TITLE);


            // 找到既有分類
            var existClassification = _MasterAggregate.KMClassification_T1_T2_.Get(x => x.ID == data.ClassificationID);

            new FileProcessInvoker((context) =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _MasterAggregate.KMData_T1_T2_.Operator(x =>
                    {
                        var dbContext = (SMARTIIEntities)x;

                        data.CreateDateTime = DateTime.Now;
                        data.CreateUserName = ContextUtility.GetUserIdentity()?.Name;
                        var entity = AutoMapper.Mapper.Map<KM_DATA>(data);

                        var result = dbContext.KM_DATA.Add(entity);

                        dbContext.SaveChanges();
                        data.ID = result.ID;

                        var pathArray = FileSaverUtility.SaveKMFiles(context, data, existClassification);

                        result.FILE_PATH = JsonConvert.SerializeObject(pathArray?.ToArray());
                        dbContext.SaveChanges();
                    });


                    scope.Complete();
                }

            });


        }

        /// <summary>
        /// 單一更新明細
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task Update(KMData data)
        {
            //重複主旨
            var isExistName = _MasterAggregate.KMData_T1_
                                                 .HasAny(x => x.ID != data.ID && x.CLASSIFICATION_ID == data.ClassificationID &&
                                                              x.TITLE == data.Title);
            if (isExistName)
                throw new Exception(KMClassification_lang.KMCLASSIFICATION_DUPLICATE_TITLE);

            // 找到既有分類
            var existClassification = _MasterAggregate.KMClassification_T1_T2_.Get(x => x.ID == data.ClassificationID);

            new FileProcessInvoker((context) =>
            {
                var pathArray = FileSaverUtility.SaveKMFiles(context, data, existClassification);

                _MasterAggregate.KMData_T1_T2_.Operator(x =>
                {
                    var dbContext = (SMARTIIEntities)x;

                    var entity = dbContext.KM_DATA.First(g => g.ID == data.ID);

                    entity.UPDATE_DATETIME = DateTime.Now;
                    entity.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                    entity.TITLE = data.Title;
                    entity.CONTENT = data.Content;

                    entity.FILE_PATH = entity.FILE_PATH.InsertArraySerialize(pathArray?.ToArray());

                    dbContext.SaveChanges();
                });
            });

        }

        /// <summary>
        /// 單一刪除明細
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task Delete(int ID)
        {
            var con = new MSSQLCondition<KM_DATA>(x => x.ID == ID);
            con.IncludeBy(x => x.KM_CLASSIFICATION);

            // 取得既有資料
            var data = _MasterAggregate.KMData_T1_T2_.Get(con);

            // 取得檔案路徑
            var paths = data?.FilePaths?.Select(path =>
                FilePathFormatted.GetKMPhysicalFilePath(
                    data.KMClassification.NodeID,
                    data.KMClassification.ID,
                    data.ID,
                    path.ParseFileName()));

            using (TransactionScope scope = new TransactionScope())
            {
                // 先刪除既有資料
                _MasterAggregate.KMData_T1_T2_.Remove(con);

                // 刪除檔案
                FileUtility.DeleteFiles(paths?.ToArray());

                scope.Complete();
            }
        }

        /// <summary>
        /// 單一新增分類
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task CreateClassification(KMClassification data)
        {
            #region 驗證名稱


            // 找到階層資訊
            var existData = _MasterAggregate.KMClassification_T1_T2_.Get(x => x.ID == data.ParentID);

            if (existData == null)
                throw new Exception(Common_lang.NOT_FOUND_DATA);



            //重複名稱
            var isExistName = _MasterAggregate.KMClassification_T1_
                                                 .HasAny(x => x.NODE_ID == existData.NodeID &&
                                                              x.ORGANIZATION_TYPE == (byte)existData.OrganizationType &&
                                                              x.NAME == data.Name);
            if (isExistName)
                throw new Exception(KMClassification_lang.KMCLASSIFICATION_DUPLICATE_NAME);
            #endregion

            data.NodeID = existData.NodeID;
            data.OrganizationType = existData.OrganizationType;


            var result = _MasterAggregate.KMClassification_T1_T2_.Add(data);
            await result.Async();
        }

        /// <summary>
        /// 單一新增分類(跟節點)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task CreateRootClassification(int nodeID, string name)
        {

            // 找到組織資訊
            var existData = _OrganizationAggregate.HeaderQuarterNode_T1_T2_.Get(x => x.NODE_ID == nodeID);

            if (existData == null)
                throw new Exception(Common_lang.NOT_FOUND_DATA);
            //重複名稱
            var isExistName = _MasterAggregate.KMClassification_T1_
                                                 .HasAny(x => x.NODE_ID == existData.NodeID &&
                                                              x.ORGANIZATION_TYPE == (byte)existData.OrganizationType &&
                                                              x.NAME == name);
            if (isExistName)
                throw new Exception(KMClassification_lang.KMCLASSIFICATION_DUPLICATE_NAME);

            var domain = new KMClassification()
            {
                NodeID = existData.NodeID,
                OrganizationType = existData.OrganizationType,
                ParentID = null,
                Name = name,

            };

            var result = _MasterAggregate.KMClassification_T1_T2_.Add(domain);
            await result.Async();
        }

        /// <summary>
        /// 更新分類名稱
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task RenameClassification(int classificationID, string name)
        {
            var con = new MSSQLCondition<KM_CLASSIFICATION>(x => x.ID == classificationID);

            var existData = _MasterAggregate.KMClassification_T1_T2_.Get(con);

            if (existData == null)
                throw new Exception(Common_lang.NOT_FOUND_DATA);

            #region 驗證名稱



            var isExistName = _MasterAggregate.KMClassification_T1_T2_
                                               .HasAny(x => x.NODE_ID != existData.NodeID &&
                                                            x.ORGANIZATION_TYPE != (byte)existData.OrganizationType &&
                                                            x.NAME == name);

            if (isExistName)
                throw new Exception(KMClassification_lang.KMCLASSIFICATION_DUPLICATE_NAME);
            #endregion

            con.ActionModify(x => x.NAME = name);

            _MasterAggregate.KMClassification_T1_.Update(con);


        }

        /// <summary>
        /// 更換分類母節點
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task DragClassification(int classificationID, int? parentID)
        {

            // 組合 con 物件
            var con = new MSSQLCondition<KM_CLASSIFICATION>(x => x.ID == classificationID);

            var existData = _MasterAggregate.KMClassification_T1_T2_.Get(con);

            if (existData == null)
                throw new Exception(Common_lang.NOT_FOUND_DATA);

            con.ActionModify(x => x.PARENT_ID = parentID);

            _MasterAggregate.KMClassification_T1_.Update(con);

        }

        /// <summary>
        /// 刪除分類
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task DeleteClassification(int ID)
        {

            var idStr = ID.ToString();

            // 找到底下的分類清單
            var vw = _MasterAggregate.VWKMClassification_KMClassification_.GetList(x => x.PARENT_PATH.Contains(idStr));
            var max = vw.OrderByDescending(x => x.Level).First();

            var regeneratorIDPath = max.ParentPath.Split(new string[] { idStr }, StringSplitOptions.None)?[0]
                .Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                .Append(idStr)
                .ToArray();


            var childrensWithSelfIDs = Array.ConvertAll(regeneratorIDPath, x => int.Parse(x));


            // 找到所有的分類
            var con = new MSSQLCondition<KM_CLASSIFICATION>(x => childrensWithSelfIDs.Contains(x.ID));
            con.IncludeBy(x => x.KM_DATA);

            var allClassification = _MasterAggregate.KMClassification_T1_T2_.GetList(con);

            // 找到所有的資料後 , 進行刪除
            var filePaths = allClassification?.SelectMany(x => x.KMDatas)
                                              .SelectMany(data =>
            {

                // 取得檔案路徑清單
                var paths = data?.FilePaths?.Select(path =>
                    FilePathFormatted.GetKMPhysicalFilePath(
                        data.KMClassification.NodeID,
                        data.KMClassification.ID,
                        data.ID,
                        path.ParseFileName()));

                return paths;
            });

            using (TransactionScope scope = new TransactionScope())
            {
                // 先刪除既有資料
                _MasterAggregate.KMClassification_T1_T2_.RemoveRange(con);

                // 刪除檔案
                FileUtility.DeleteFiles(filePaths?.ToArray());

                scope.Complete();
            }

        }

        /// <summary>
        /// 刪除附件
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        public void DeleteFileWithUpdate(int id, string key)
        {
            var con = new MSSQLCondition<KM_DATA>(x => x.ID == id);
            con.IncludeBy(x => x.KM_CLASSIFICATION);
            con.ActionModify(x =>
            {
                x.FILE_PATH = x.FILE_PATH.RemoveArraySerialize(key);
            });

            using (TransactionScope scope = new TransactionScope())
            {
                var data = _MasterAggregate.KMData_T1_T2_.Update(con);

                var path = FilePathFormatted.GetKMPhysicalFilePath(
                        data.KMClassification.NodeID,
                        data.KMClassification.ID,
                        data.ID,
                        key);

                FileUtility.DeleteFile(path);

                scope.Complete();
            }
        }

    }
}
