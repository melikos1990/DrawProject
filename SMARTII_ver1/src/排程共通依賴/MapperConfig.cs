using Autofac;
using SMARTII.Assist.Mapper;
using SMARTII.Domain.Mapper;
using System.Collections.Generic;
using System.Linq;

namespace 排程共通依賴
{
    public static class MapperConfig
    {

        public static void Init(IContainer container)
        {

            // 這邊透過容器取得 IProfileExpression 介面實作的對象
            // 通常是其他Bu 的 dll 實作該介面 , 各自定義AutoMapper Profiles
            var expressions = container.Resolve<IEnumerable<IProfileExpression>>();

            // 這邊將各自定義的 AutoMapper profiles 進行註冊
            EntryProfile.Initialize(expressions.ToArray());
        }
    }
}
