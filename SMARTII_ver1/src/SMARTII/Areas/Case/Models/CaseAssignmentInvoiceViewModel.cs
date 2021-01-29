using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SMARTII.Domain.Case;
using SMARTII.Domain.Data;
using SMARTII.Domain.Organization;

namespace SMARTII.Areas.Case.Models
{
    public class CaseAssignmentInvoiceViewModel : CaseAssignmentBaseViewModel
    {
        public CaseAssignmentInvoiceViewModel()
        {

        }

        public CaseAssignmentInvoiceViewModel(CaseAssignmentComplaintInvoice invoice)
        {
            this.ID = invoice.ID;
            this.CaseAssignmentComplaintInvoiceType = invoice.CaseAssignmentComplaintInvoiceType;
            this.CaseAssignmentComplaintInvoiceTypeName = invoice.CaseAssignmentComplaintInvoiceType.GetDescription();
            this.CaseAssignmentProcessType = invoice.CaseAssignmentProcessType;
            this.CaseAssignmentProcessTypeName = invoice.CaseAssignmentProcessType.GetDescription();
            this.CaseID = invoice.CaseID;
            this.Content = invoice.Content;
            this.CreateDateTime = invoice.CreateDateTime;
            this.CreateUserName = invoice.CreateUserName;
            this.EMLFilePath = invoice.EMLFilePath;
            this.FilePath = invoice.FilePath;
            this.InvoiceID = invoice.InvoiceID;
            this.InvoiceType = invoice.InvoiceType;
            this.IsRecall = invoice.IsRecall;
            this.NodeID = invoice.NodeID;
            this.NotificationUsers = string.Join(",", invoice.NoticeUsers ?? new string[] { });
            this.NotificationBehaviors = invoice.NotificationBehaviors;
            this.NotificationDateTime = invoice.NotificationDateTime;
            this.OrganizationType = invoice.OrganizationType;
            this.Users = invoice.Users?
                                .Select(x => new CaseAssignmentInvoiceUserViewModel(x))
                                .ToList();




        }

        public CaseAssignmentComplaintInvoice ToDomain()
        {
            var invoice = new CaseAssignmentComplaintInvoice();
            invoice.ID = this.ID;
            invoice.CaseAssignmentComplaintInvoiceType = this.CaseAssignmentComplaintInvoiceType;
            invoice.CaseAssignmentProcessType = this.CaseAssignmentProcessType;
            invoice.CaseID = this.CaseID;
            invoice.Content = this.Content;
            invoice.CreateDateTime = this.CreateDateTime;
            invoice.CreateUserName = this.CreateUserName;
            invoice.EMLFilePath = this.EMLFilePath;
            invoice.FilePath = this.FilePath;
            invoice.InvoiceID = this.InvoiceID;
            invoice.InvoiceType = this.InvoiceType;
            invoice.IsRecall = this.IsRecall;
            invoice.NodeID = this.NodeID;
            invoice.NoticeUsers = this.NotificationUsers?.Split(',');
            invoice.NotificationBehaviors = this.NotificationBehaviors;
            invoice.NotificationDateTime = this.NotificationDateTime;
            invoice.OrganizationType = this.OrganizationType;
            invoice.Files = this.Files;
            invoice.Users = this.Users?
                                .Select(x => x.ToDomain())
                                .ToList();
            return invoice;

        }

        /// <summary>
        /// 流水號
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 反應單號
        /// </summary>
        public string InvoiceID { get; set; }

        /// <summary>
        /// 反應單型態 (行銷/...)
        /// </summary>
        public string InvoiceType { get; set; }
        /// <summary>
        /// 反應單型態名稱
        /// </summary>
        public string InvoiceTypeName { get; set; }

        /// <summary>
        /// 反應單狀態
        /// </summary>
        public CaseAssignmentComplaintInvoiceType CaseAssignmentComplaintInvoiceType { get; set; }

        /// <summary>
        /// 反應單狀態
        /// </summary>
        public string CaseAssignmentComplaintInvoiceTypeName { get; set; }

        /// <summary>
        /// 是否需回電
        /// </summary>
        public bool IsRecall { get; set; }

        /// <summary>
        /// 底下的人員
        /// </summary>
        public List<CaseAssignmentInvoiceUserViewModel> Users { get; set; }



    }
}
