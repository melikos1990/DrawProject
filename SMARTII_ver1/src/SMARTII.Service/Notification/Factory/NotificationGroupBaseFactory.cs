using System;
using System.Collections.Generic;
using System.Linq;
using Ptc.Data.Condition2.Mssql.Class;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Notification;

namespace SMARTII.Service.Notification.Factory
{
    public class NotificationGroupBaseFactory
    {
        public NotificationGroupEffectiveSummary GenerateSummary(NotificationGroup group, List<Domain.Case.Case> actualCases)
        {
            return new NotificationGroupEffectiveSummary()
            {
                GroupID = group.ID,
                ActualArriveCount = actualCases.Count(),
                ExpectArriveCount = group.AlertCount,
                CaseIDs = actualCases.Select(x => x.CaseID)?.ToArray(),
                CreateDateTime = DateTime.Now,
            };
        }

        public MSSQLCondition<NOTIFICATION_GROUP> GenerateCondition(NotificationGroup group, List<Domain.Case.Case> actualCases, bool isArrive)
        {
            var condition = new MSSQLCondition<NOTIFICATION_GROUP>(x => x.ID == group.ID);

            condition.ActionModify(x =>
            {
                x.UPDATE_DATETIME = DateTime.Now;
                x.UPDATE_USERNAME = GlobalizationCache.APName;
                x.IS_ARRIVE = isArrive;
                x.ACTUAL_COUNT = actualCases.Count();

            // 曾經達標過?

            if (x.IS_NOTIFICATED == false && x.IS_ARRIVE)
                    x.IS_NOTIFICATED = true;
            });

            return condition;
        }

        public List<string> GetUserIDs(NotificationGroup group)
        {
            var userIDs = group.NotificationGroupUsers?
                                    .Where(x => string.IsNullOrEmpty(x.UserID) == false)?
                                    .Select(x => x.UserID)
                                    .ToList() ?? new List<string>();

            return userIDs;
        }
    }
}
