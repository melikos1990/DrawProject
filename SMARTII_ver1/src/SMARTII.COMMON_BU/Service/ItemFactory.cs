using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Data;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.Resource.Tag;

namespace SMARTII.COMMON_BU.Service
{
    public class ItemFactory : IItemFactory
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public ItemFactory(
            IMasterAggregate MasterAggregate,
            IOrganizationAggregate OrganizationAggregate
        )
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }


        public IEnumerable<DataRow> GetCurrentRows(DataTable table) => table.Select().ToList()?.Skip(2);

        public async Task Create<T>(Item item)
        {
           

            new FileProcessInvoker((context) =>
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _MasterAggregate.Billboard_T1_T2_.Operator(g =>
                    {
                        var dbContext = (SMARTIIEntities)g;

                        var hasBu = dbContext.HEADQUARTERS_NODE.Any(x => x.NODE_ID == item.NodeID);
                        if (hasBu == false)
                            throw new Exception(Common_lang.BUSINESSS_NOT_FOUND);

                        item.CreateDateTime = DateTime.Now;
                        item.CreateUserName = ContextUtility.GetUserIdentity().Name;

                        var entity = AutoMapper.Mapper.Map<ITEM>(item);

                        var result = dbContext.ITEM.Add(entity);
                        dbContext.SaveChanges();

                        // 先將 ID 回填後進行圖片存檔/寫入
                        item.ID = result.ID;

                        var pathArray = FileSaverUtility.SaveItemFiles(context, item);
                        result.IMAGE_PATH = JsonConvert.SerializeObject(pathArray?.ToArray());
                        dbContext.SaveChanges();
                    });

                    scope.Complete();
                }
            });
        }

        public async Task Update<T>(Item item)
        {
            var con = new MSSQLCondition<ITEM>(x => x.ID == item.ID);

          

            new FileProcessInvoker((context) =>
            {
                var pathArray = FileSaverUtility.SaveItemFiles(context, item);

                _MasterAggregate.Item_T1_T2_.Operator(g =>
                {
                    var dbContext = (SMARTIIEntities)g;
                    
                    var hasBu = dbContext.HEADQUARTERS_NODE.Any(x => x.NODE_ID == item.NodeID);
                    if (hasBu == false)
                        throw new Exception(Common_lang.BUSINESSS_NOT_FOUND);

                    var entity = dbContext.ITEM.First(x => x.ID == item.ID);

                    entity.UPDATE_DATETIME = DateTime.Now;
                    entity.UPDATE_USERNAME = ContextUtility.GetUserIdentity().Name;
                    entity.NAME = item.Name;
                    entity.DESCRIPTION = item.Description;
                    entity.J_CONTENT = (item as Item<T>)?.JContent;
                    entity.CODE = item.Code;
                    entity.IS_ENABLED = item.IsEnabled;
                    entity.IMAGE_PATH = entity.IMAGE_PATH.InsertArraySerialize(pathArray?.ToArray());

                    dbContext.SaveChanges();
                });
            });
        }

        public void DeleteImage(int id, string key)
        {
            var con = new MSSQLCondition<ITEM>(x => x.ID == id);

            con.ActionModify(x =>
            {
                x.IMAGE_PATH = x.IMAGE_PATH.RemoveArraySerialize(key);
            });

            using (TransactionScope scope = new TransactionScope())
            {
                var data = _MasterAggregate.Item_T1_T2_.Update(con);

                var path = FilePathFormatted.GetItemPhysicalFilePath(
                        data.NodeID,
                        data.ID,
                        key);

                FileUtility.DeleteFile(path);

                scope.Complete();
            }
        }

        public virtual bool Import(DataTable data, ref ErrorContext<Item> errorContext)
        {

            var bu = _OrganizationAggregate.HeaderQuarterNode_T1_.Get(x => x.NODE_KEY == data.TableName);

            var domains = GetCurrentRows(data).Select(row => new Item()
                            {
                                NodeID = bu.NODE_ID,
                                NodeName = bu.NAME,
                                Name = row[1].ToString(),
                                Code = row[2].ToString(),
                                Description = row[3].ToString(),
                                IsEnabled = Convert.ToBoolean(row[4].ToString() == "啟用" ? true : false),
                                CreateDateTime = DateTime.Now,
                                CreateUserName = ContextUtility.GetUserIdentity().Name,
                            });

            // 加入驗證失敗資料
            errorContext.AddRange(this.ValidData(domains));
            

            // 驗證商品欄位 
            if (errorContext.InValid())
            {
                return false;
            }

            foreach (var domain in domains)
            {
                try
                {
                    _MasterAggregate.Item_T1_T2_.Add(domain);
                }
                catch (System.Exception ex)
                {
                    errorContext.Add(domain);
                }
            }




            return !errorContext.InValid();
        }


        /// <summary>
        /// 驗證資料是否符合規定
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="errorCollection"></param>
        /// <returns></returns>
        protected virtual List<Item> ValidData(IEnumerable<Item> datas)
        {
            return datas.Where(data =>
                    string.IsNullOrEmpty(data.Name) ||
                    data.IsEnabled == null
                ).ToList();
        }
    }
}
