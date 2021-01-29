using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ptc.Data.Condition2;
using SMARTII.Domain.Notification;

namespace SMARTII.Tests
{
    /// <summary>
    /// NotificationTest 的摘要說明
    /// </summary>
    [TestClass]
    public class NotificationTest
    {
        public NotificationTest()
        {
            // 初始化Autofac
            var container = DIConfig.Init();

            // 初始化AutoMapper
            MapperConfig.Init(container);

            // 初始化MSSQL CONDITION
            DataAccessConfiguration.Configure(Condition2Config.Setups);

            DIBuilder.SetContainer(container);
        }
        
        
        [TestMethod]
        public void TestMethod1()
        {
            var notificationPersonal = DIBuilder.Resolve<INotificationPersonalFacade>();

            notificationPersonal.BillBoardNotification();
        }
    }
}
