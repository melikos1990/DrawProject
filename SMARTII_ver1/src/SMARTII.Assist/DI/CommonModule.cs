using System.Linq;
using Autofac;
using Autofac.Extras.AggregateService;
using MoreLinq;
using NLog;
using NLog.Targets;
using NLog.Targets.Wrappers;
using SMARTII.Assist.Logger;
using SMARTII.Domain.Cache;
using SMARTII.Domain.Common;
using SMARTII.Domain.Data;

namespace SMARTII.Assist.DI
{
    public class CommonModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAggregateService<ICommonAggregate>();

            var databaseTarget = new DatabaseTarget(); ;
            var targetWrapper = LogManager.Configuration.FindTargetByName("Database");

            databaseTarget = targetWrapper as DatabaseTarget;

            var poss = DataAccessCache.Instance.SmartIIConn+"@";
            var rightKey = "\"@";
            var leftKey = "connection string=\"";

            // 取得 Keyword poisition
            var left_index = poss.IndexOf(leftKey);
            var right_index = poss.IndexOf(rightKey);
            

            // 計算 擷取字串長度
            var offset = (right_index - left_index) + rightKey.Length;

            // 取得 Tag
            var tag = poss.Substring(left_index, offset);
            

            var connString = tag.Replace(leftKey, "").Replace(rightKey, "");

            databaseTarget.ConnectionString = connString;
            LogManager.ReconfigExistingLoggers();

            var loggerKey = new string[] {
                "Email" ,
                "Database" ,
                "System",  //<== default
            };

            var instance = Ptc.Logger.NLogManager.CreateMutipleInstance(loggerKey);

            instance.LoggerDist.ForEach(x =>
            {
                builder.Register(g => x.Value)
                       .As<Ptc.Logger.ILogger>()
                       .Keyed<Ptc.Logger.ILogger>(x.Key)
                       .InstancePerLifetimeScope();
            });

            builder.RegisterType<LoggerAttribute>().PropertiesAutowired();
        }
    }
}
