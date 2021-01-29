#define Test

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Autofac.Features.Indexed;
using MultipartDataMediaFormatter.Infrastructure;
using Newtonsoft.Json;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Case;
using SMARTII.Domain.IO;
using SMARTII.Domain.Master;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Notification.Email;
using SMARTII.Domain.Thread;
using SMARTII.Domain.Transaction;
using SMARTII.Resource.Tag;
using SMARTII.Service.Organization.Provider;

namespace SMARTII.Service.Case.Task
{
    public class AssignNotificationNoSendTask : IFlowableTask
    {
        private readonly ICaseAggregate _CaseAggregate;

        public AssignNotificationNoSendTask(ICaseAggregate CaseAggregate)
        {
            _CaseAggregate = CaseAggregate;
        }

        public async Task<IFlowable> Execute(IFlowable flowable, params object[] args)
        {
            IFlowable result = null;

            var data = (CaseAssignmentBase)flowable;

            //新增一般通知歷程
            var assignComplaintNoticeDomain = new CaseAssignmentComplaintNotice()
            {
                CaseID = data.CaseID,
                NodeID = data.NodeID,
                OrganizationType = data.OrganizationType,
                Content = data.Content,
                FilePath = new string[] { data.EMLFilePath },
                NotificationDateTime = DateTime.Now,
                CreateDateTime = DateTime.Now,
                CreateUserName = ContextUtility.GetUserIdentity()?.Name
            };

            // 寫入資料
            result = _CaseAggregate.CaseAssignmentComplaintNotice_T1_T2_.Add(assignComplaintNoticeDomain);

            return result;
        }

        public async global::System.Threading.Tasks.Task Validator(IFlowable flowable, params object[] args)
        {

        }
    }
}
