using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Database.SMARTII;

namespace SMARTII.Areas.Common.Models.Master
{
    public class CaseTemplateParseViewModel
    {


        /// <summary>
        /// 組織代號
        /// </summary>
        [MSSQLFilter(nameof(CASE_TEMPLATE.NODE_ID),
        ExpressionType.Equal,
        PredicateType.And)]
        public int? NodeID { get; set; }
        

        /// <summary>
        /// key
        /// </summary>
        [MSSQLFilter(nameof(CASE_TEMPLATE.ID) , 
        ExpressionType.Equal , 
        PredicateType.And)]
        public int? CaseTemplateID { get; set; }

        /// <summary>
        /// 使用時機
        /// </summary>
        [MSSQLFilter(nameof(CASE_TEMPLATE.CLASSIFIC_KEY),
        ExpressionType.Equal,
        PredicateType.And)]
        public string ClassificKey { get; set; }

        /// <summary>
        /// 是否為預設
        /// </summary>
        [MSSQLFilter(nameof(CASE_TEMPLATE.IS_DEFAULT),
        ExpressionType.Equal,
        PredicateType.And)]
        public bool? IsDefault  { get; set; }

        #region 後續計算上使用

        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }

        /// <summary>
        /// 反應單號
        /// </summary>
        public string InvoicID { get; set; }

        /// <summary>
        /// 轉派ID
        /// </summary>
        public int? AssignmentID { get; set; }
        #endregion


    }
}
