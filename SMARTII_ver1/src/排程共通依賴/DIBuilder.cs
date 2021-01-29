﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using SMARTII._21Century.DI;
using SMARTII.ASO.DI;
using SMARTII.Assist.DI;
using SMARTII.ColdStone.DI;
using SMARTII.COMMON_BU.DI;
using SMARTII.Configuration.DI;
using SMARTII.EShop.DI;
using SMARTII.ICC.DI;
using SMARTII.MisterDonut.DI;
using SMARTII.OpenPoint.DI;
using SMARTII.PPCLIFE.DI;
using SMARTII.FORTUNE.DI;

namespace 排程共通依賴
{
    public static class DIBuilder
    {
        private static IContainer _Container;

        private static void Initialize()
        {
            EntryBuilder.RegisterModule(new COMMON_BUModule());
            EntryBuilder.RegisterModule(new PPCLIFEModule());
            EntryBuilder.RegisterModule(new ColdStoneModule());
            EntryBuilder.RegisterModule(new MainModule());
            EntryBuilder.RegisterModule(new ASOModule());
            EntryBuilder.RegisterModule(new MisterDonutModule());
            EntryBuilder.RegisterModule(new _21CenturyModule());
            EntryBuilder.RegisterModule(new EShopModule());
            EntryBuilder.RegisterModule(new OpenPointModule());
            EntryBuilder.RegisterModule(new ICCModule());
            EntryBuilder.RegisterModule(new FORTUNEModule());
            EntryBuilder.RegisterModule(new WebModule());

            _Container = EntryBuilder.Build();
        }

        public static void SetContainer(IContainer container)
        {
            _Container = container;
        }

        public static T Resolve<T>()
        {
            if (_Container == null)
                Initialize();

            return _Container.Resolve<T>();
        }
    }
}



