using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ptc.Data.Condition2.Interface.Type;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Notification;
using SMARTII.Domain.Organization;
using SMARTII.Domain.Thread;

namespace SMARTII.Domain.Data
{
    public static class ConditionUtility
    {
        public static void FilterNodeFromPosition<T>(this MSSQLCondition<T> condition, List<JobPosition> jobPositions) where T : NODE, new()
        {
            jobPositions.ForEach(position =>
            {
                condition.Or(x =>
                x.ORGANIZATION_TYPE == (byte)position.OrganizationType &&
                x.LEFT_BOUNDARY >= position.LeftBoundary &&
                x.RIGHT_BOUNDARY <= position.RightBoundary);
            });
        }

        public static void FilterCallCenterTreeNodeRange(this MSSQLCondition<CALLCENTER_NODE> condition, OrganizationType goal)
        {
            var user = ContextUtility.GetUserIdentity().Instance;

            var providerBUs = user.DownProviderBUDist.TryGetBuList(goal).Cast<int?>();

            switch (goal)
            {
                case OrganizationType.HeaderQuarter:
                    condition.Or(x => x.HEADQUARTERS_NODE.Any(g => providerBUs.Contains(g.NODE_ID)));
                    break;

                case OrganizationType.CallCenter:
                    condition.FilterNodeFromPosition(user.JobPositions);
                    break;

                case OrganizationType.Vendor:
                    break;

                default:
                    break;
            }

            condition.Or(x => x.LEFT_BOUNDARY == 1);
        }

        public static void FilterHeaderQuartersTreeNodeRange(this MSSQLCondition<HEADQUARTERS_NODE> condition, OrganizationType goal)
        {
            var user = ContextUtility.GetUserIdentity().Instance;

            var providerBUs = user.DownProviderBUDist.TryGetBuList(goal).Cast<int?>();

            switch (goal)
            {
                case OrganizationType.HeaderQuarter:
                    condition.FilterNodeFromPosition(user.JobPositions);
                    break;

                case OrganizationType.CallCenter:
                    condition.Or(x => providerBUs.Contains(x.BU_ID));
                    break;

                case OrganizationType.Vendor:
                    condition.Or(x => providerBUs.Contains(x.BU_ID));
                    break;

                default:
                    break;
            }

            condition.Or(x => x.LEFT_BOUNDARY == 1);
        }

        public static void FilterVendorTreeNodeRange(this MSSQLCondition<VENDOR_NODE> condition, OrganizationType goal)
        {
            var user = ContextUtility.GetUserIdentity().Instance;

            var providerBUs = user.DownProviderBUDist.TryGetBuList(goal).Cast<int?>();

            switch (goal)
            {
                case OrganizationType.HeaderQuarter:
                    condition.Or(x => x.HEADQUARTERS_NODE.Any(g => providerBUs.Contains(g.NODE_ID)));
                    break;

                case OrganizationType.CallCenter:
                    condition.Or(x => x.HEADQUARTERS_NODE.Any(g => providerBUs.Contains(g.NODE_ID)));
                    break;

                case OrganizationType.Vendor:
                    condition.FilterNodeFromPosition(user.JobPositions);
                    break;

                default:
                    break;
            }

            condition.Or(x => x.LEFT_BOUNDARY == 1);
        }

        public static void FilterUserFromPosition<T>(this MSSQLCondition<USER> condition, List<JobPosition> jobPositions) where T : JobPosition
        {
            jobPositions.OfType<T>()
                   .Select(x => new
                   {
                       left = x.LeftBoundary,
                       right = x.RightBoundary,
                       organizationType = x.OrganizationType
                   })
                   .ToList()
                   .ForEach(x =>
                   {
                       condition.Or(c =>
                         c.NODE_JOB.Any(p =>
                              p.ORGANIZATION_TYPE == (byte)x.organizationType &&
                              p.LEFT_BOUNDARY >= x.left &&
                              p.RIGHT_BOUNDARY <= x.right

                       ));
                   });
        }

        public static void FilterSpecifyNodeField<T>(this MSSQLCondition<T> condition, 
            Expression<Func<T, bool>> Expression, 
            PredicateType Type = PredicateType.And) where T : NODE, new()
        {
            if (Type == PredicateType.And)
            {
                condition.And(Expression);
            }
            else if (Type == PredicateType.Or)
            {
                condition.Or(Expression);
            }

            condition.Or(x => x.LEFT_BOUNDARY == 1);

        }

        public static IEnumerable<T> Query<T>(this IEnumerable<T> collection, MSSQLCondition<T> con) where T : class , new()
        {
            var filters = con.GetFilters()?
                             .Select(x => x.Expression.Compile())
                             .ToList();

            foreach (var filter in filters)
            {
                collection = collection.Where(filter).ToList();
            }

            return collection;

        }

        public static void ComplainedSelfFromPosition<T>(this MSSQLCondition<CASE> condition, List<JobPosition> jobPositions) where T : JobPosition, new()
        {
            jobPositions
                .OfType<T>()
                .ToList()
                .ForEach(nodeJob => {
                    condition.Or(x => x.CASE_COMPLAINED_USER.Any(g => g.NODE_ID == nodeJob.NodeID));
                });
        }


        public static void SystemNotificaitonFromPersonal(this MSSQLCondition<PERSONAL_NOTIFICATION> condition)
        {
            condition.Or(x => x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.Billboard);
            condition.Or(x => x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.NotificationGroup);
            condition.Or(x => x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.CaseAssign);
            condition.Or(x => x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.CaseAssignmentFinish);
            condition.Or(x => x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.MailAdopt);
            condition.Or(x => x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.CaseModify);
            condition.Or(x => x.PERSONAL_NOTIFICATION_TYPE == (byte)PersonalNotificationType.NotificationPPCLife);
        }

    }
}
