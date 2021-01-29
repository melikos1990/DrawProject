using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using SMARTII.Domain.Data;
using SMARTII.Domain.Master;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;
using SMARTII.PPCLIFE.Domain;
using SMARTII.Resource.Tag;

namespace SMARTII.PPCLIFE.Service
{
    /// <summary>
    /// 選擇性實作主介面
    /// 父類別處理共通項目
    /// </summary>
    public class ItemFactory : SMARTII.COMMON_BU.Service.ItemFactory, IItemFactory
    {
        private readonly IMasterAggregate _MasterAggregate;
        private readonly IOrganizationAggregate _OrganizationAggregate;

        public ItemFactory(
            IMasterAggregate MasterAggregate,
            IOrganizationAggregate OrganizationAggregate
        ) : base(MasterAggregate, OrganizationAggregate)
        {
            _MasterAggregate = MasterAggregate;
            _OrganizationAggregate = OrganizationAggregate;
        }

  

        public override bool Import(DataTable data, ref ErrorContext<Item> errorContext)
        {
            var bu = _OrganizationAggregate.HeaderQuarterNode_T1_.Get(x => x.NODE_KEY == data.TableName);


            var domains = GetCurrentRows(data).Select(row => new Item<PPCLIFE_Item>()
                           {
                               NodeID = bu.NODE_ID,
                               NodeName = bu.NAME,
                               Name = row[1].ToString(),
                               Code = row[2].ToString(),
                               Description = row[3].ToString(),
                               IsEnabled = Convert.ToBoolean(row[4].ToString() == "啟用" ? true : false),
                               CreateDateTime = DateTime.Now,
                               CreateUserName = ContextUtility.GetUserIdentity().Name,
                               Particular = new PPCLIFE_Item()
                               {
                                   InternationalBarcode = row[5].ToString(),
                               }
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


    }
}
