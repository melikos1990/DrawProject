using System.Collections.Generic;
using System.Linq;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Domain.Master
{
    public class KMClassification : IRecursivelyModel, IOrganizationRelationship
    {
        public KMClassification()
        {
        }

        /// <summary>
        /// 識別直
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 分類名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父分類
        /// </summary>
        public KMClassification Parent { get; set; }

        /// <summary>
        /// 底下的細項
        /// </summary>
        public List<KMData> KMDatas { get; set; }

        #region impl

        /// <summary>
        /// 組織型態
        /// </summary>
        public OrganizationType OrganizationType { get; set; }

        /// <summary>
        /// 組織代號 (企業別)
        /// </summary>
        public int NodeID { get; set; }

        /// <summary>
        /// 組織名稱
        /// </summary>
        public string NodeName { get; set; }

        /// <summary>
        /// 組織節點
        /// </summary>
        public IOrganizationNode Node { get; set; }



        public int? ParentID { get; set; }

        public int? ParentLocator
        {
            get
            {
                return ParentID;
            }
            set
            {
                ParentID = value;
            }
        }

        #endregion impl

        #region View Table

        /// <summary>
        /// 層級數
        /// </summary>
        public int Level { get; set; }


        /// <summary>
        /// 父節點的 名稱 路徑
        /// </summary>
        public string ParentNamePath { get; set; }

        /// <summary>
        /// 父節點的 ID 路徑
        /// </summary>
        public string ParentPath { get; set; }

        /// <summary>
        /// 父節點 的 名稱路徑清單
        /// </summary>
        public string[] ParentNamePathByArray { get { return this.ParentNamePath?.Split('/') ?? new string[] { }; } }

        /// <summary>
        /// 父節點 的 代號路徑清單
        /// </summary>
        public string[] ParentPathByArray { get { return this.ParentPath?.Split('/') ?? new string[] { }; } }

        public List<IRecursivelyModel> Children { get; set; } 
        

        #endregion View Table
    }
}
