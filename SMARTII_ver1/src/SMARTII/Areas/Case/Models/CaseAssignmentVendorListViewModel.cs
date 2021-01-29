using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Attribute;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Report;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentVendorListViewModel
    {
        public CaseAssignmentVendorListViewModel()
        { }

        public CaseAssignmentVendorListViewModel(ExcelCaseAssignmentList x)
        {
            CaseAssignmentProcessType = x.CaseAssignmentProcessType;
            NoticeDateTime = x.NoticeTime;
            CaseID = x.CaseID;
            SN = x.SN;
            Mode = x.ModeType;
            Type = x.AssignmentType;
            CaseAssignmentUser = x.AssignmentUser == null ? "" : string.Join(" / ", x.AssignmentUser.ToArray());
            ConcatUserName = x.ConcatUnitType == UnitType.Customer.GetDescription() ? x.ConcatCustomerName :
            x.ConcatUnitType == UnitType.Store.GetDescription() ? x.ConcatStore : x.ConcatOrganization;
            ComplainedUserName = x.ComplainedUnitType == UnitType.Store.GetDescription() ? x.ComplainedStore : x.ComplainedOrganizationNodeName;
            ComplainedUserParentNamePath = x.ComplainedOrganization;
            CaseContent = x.CaseContent;
            NoticeContent = x.NoticeContent;
            IdentityID = x.IdentityID;
            IsRowFocus = x.CaseAssignmentProcessType == CaseAssignmentProcessType.Assignment && Convert.ToInt32(x.SN) >= 2 ? true : false;
        }


        /// <summary>
        /// 通知時間
        /// </summary>
        public string NoticeDateTime { get; set; }
        /// <summary>
        /// 案件編號
        /// </summary>
        public string CaseID { get; set; }
        /// <summary>
        /// 序號
        /// </summary>
        public string SN { get; set; }
        /// <summary>
        /// 歷程模式
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 歷程狀態
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 轉派對象
        /// </summary>
        public string CaseAssignmentUser { get; set; }
        /// <summary>
        /// 反應者
        /// </summary>
        public string ConcatUserName { get; set; }
        /// <summary>
        /// 被反應者
        /// </summary>
        public string ComplainedUserName { get; set; }
        /// <summary>
        /// 被反應者組織
        /// </summary>
        public string ComplainedUserParentNamePath { get; set; }
        /// <summary>
        /// 案件內容
        /// </summary>
        public string CaseContent { get; set; }
        /// <summary>
        /// 通知內容
        /// </summary>
        public string NoticeContent { get; set; }

        /// <summary>
        /// 歷程模式(type)
        /// <summary>
        public CaseAssignmentProcessType CaseAssignmentProcessType { get; set; }


        /// <summary>
        /// 歷程識別值(反應單/一般通知)
        /// </summary>
        public int? IdentityID { get; set; }

        /// <summary>
        /// 查詢結果清單是否標示
        /// </summary>
        public bool IsRowFocus { get; set; }
    }
}
