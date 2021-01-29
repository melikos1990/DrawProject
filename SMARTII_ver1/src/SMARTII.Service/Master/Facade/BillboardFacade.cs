using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using MoreLinq;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.Service.Master.Facade
{
    public class BillboardFacade : IBillboardFacade
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public BillboardFacade(IMasterAggregate MasterAggregate,
                                IOrganizationAggregate OrganizationAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

        /// <summary>
        /// 單一取得
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<Billboard> Get(int ID)
        {
            var data = _MasterAggregate.Billboard_T1_T2_.Get(x => x.ID == ID);

            var userIDs = data?.UserIDs ?? new List<string>();

            var users = _OrganizationAggregate.User_T1_T2_.GetList(x => userIDs.Contains(x.USER_ID));

            data.Users = users.ToList();

            return data;
        }

        /// <summary>
        /// 單一新增
        /// </summary>
        /// <param name="data"></param>
        public async Task Create(Billboard data)
        {
            var users = _OrganizationAggregate.User_T1_T2_
                                              .GetList(x => data.UserIDs.Contains(x.USER_ID))
                                              .ToList();

            if (!users.All(UserUtility.ValidExpression()))
                throw new Exception(Common_lang.USER_UNACTIVE);

            if (users.IsDuplicate())
                throw new Exception(Common_lang.USER_DUPLICATE);

            new FileProcessInvoker((context) =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _MasterAggregate.Billboard_T1_T2_.Operator(x =>
                    {
                        var dbContext = (SMARTIIEntities)x;

                        data.CreateDateTime = DateTime.Now;
                        data.CreateUserID = ContextUtility.GetUserIdentity().Instance.UserID;
                        data.CreateUserName = ContextUtility.GetUserIdentity().Name;

                        var entity = AutoMapper.Mapper.Map<BILL_BOARD>(data);
                        var result = dbContext.BILL_BOARD.Add(entity);
                        dbContext.SaveChanges();

                        // 先將 ID 回填後進行圖片存檔/寫入
                        data.ID = result.ID;

                        var pathArray = FileSaverUtility.SaveBillboardFiles(context, data);
                        result.FILE_PATH = JsonConvert.SerializeObject(pathArray?.ToArray());
                        dbContext.SaveChanges();
                    });

                    scope.Complete();
                }
            });
        }

        /// <summary>
        /// 單一更新
        /// </summary>
        /// <param name="data"></param>
        public async Task Update(Billboard data)
        {
            var users = _OrganizationAggregate.User_T1_T2_
                                              .GetList(x => data.UserIDs.Contains(x.USER_ID))
                                              .ToList();

            if (!users.All(UserUtility.ValidExpression()))
                throw new Exception(Common_lang.USER_UNACTIVE);

            if (users.IsDuplicate())
                throw new Exception(Common_lang.USER_DUPLICATE);

            new FileProcessInvoker((context) =>
            {
                var pathArray = FileSaverUtility.SaveBillboardFiles(context, data);

                _MasterAggregate.Billboard_T1_T2_.Operator(x =>
                {
                    var dbContext = (SMARTIIEntities)x;

                    var entity = dbContext.BILL_BOARD.First(g => g.ID == data.ID);

                    entity.UPDATE_DATETIME = DateTime.Now;
                    entity.UPDATE_USER_ID = ContextUtility.GetUserIdentity().Instance.UserID;
                    entity.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                    entity.USER_IDs = JsonConvert.SerializeObject(data.UserIDs);
                    entity.WARNING_TYPE = (byte)data.BillboardWarningType;
                    entity.TITLE = data.Title;
                    entity.CONTENT = data.Content;
                    entity.ACTIVE_DATE_END = data.ActiveEndDateTime;
                    entity.ACTIVE_DATE_START = data.ActiveStartDateTime;
                    entity.FILE_PATH = entity.FILE_PATH.InsertArraySerialize(pathArray?.ToArray());

                    dbContext.SaveChanges();
                });
            });
        }

        /// <summary>
        /// 單一刪除
        /// </summary>
        /// <param name="ID"></param>
        public async Task Delete(int ID)
        {
            var con = new MSSQLCondition<BILL_BOARD>(x => x.ID == ID);

            // 取得既有資料
            var data = _MasterAggregate.Billboard_T1_T2_.Get(con);

            // 取得檔案路徑
            var paths = data?.FilePaths?.Select(path =>
                FilePathFormatted.GetBillboardPhysicalFilePath(
                    data.CreateDateTime.ToString("yyyyMMdd"),
                    data.ID,
                    path.ParseFileName()));

            using (TransactionScope scope = new TransactionScope())
            {
                // 先刪除既有資料
                _MasterAggregate.Billboard_T1_T2_.Remove(con);

                // 刪除檔案
                FileUtility.DeleteFiles(paths?.ToArray());

                scope.Complete();
            }
        }

        /// <summary>
        /// 批次刪除
        /// </summary>
        /// <param name="IDs"></param>
        public async Task DeleteRange(int[] IDs)
        {
            var con = new MSSQLCondition<BILL_BOARD>(x => IDs.Contains(x.ID));

            // 取得既有資料
            var collection = _MasterAggregate.Billboard_T1_T2_.GetList(con);

            var paths = collection.SelectMany(data =>
                        {
                            return data.FilePaths?.Select(path =>
                                FilePathFormatted.GetBillboardPhysicalFilePath(
                                data.CreateDateTime.ToString("yyyyMMdd"),
                                data.ID,
                                path.ParseFileName()));
                        });

            using (TransactionScope scope = new TransactionScope())
            {
                // 先刪除既有資料
                _MasterAggregate.Billboard_T1_T2_.RemoveRange(con);

                // 刪除檔案
                FileUtility.DeleteFiles(paths?.ToArray());

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
            var con = new MSSQLCondition<BILL_BOARD>(x => x.ID == id);

            con.ActionModify(x =>
            {
                x.FILE_PATH = x.FILE_PATH.RemoveArraySerialize(key);
            });

            using (TransactionScope scope = new TransactionScope())
            {
                var data = _MasterAggregate.Billboard_T1_T2_.Update(con);

                var path = FilePathFormatted.GetBillboardPhysicalFilePath(
                        data.CreateDateTime.ToString("yyyyMMdd"),
                        data.ID,
                        key);

                FileUtility.DeleteFile(path);

                scope.Complete();
            }
        }

       
    }
}
