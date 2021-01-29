using Microsoft.CodeAnalysis.Scripting;
using Ptc.Data.Condition2.Interface.Common;
using Ptc.Data.Condition2.Mssql.Common;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;

namespace SMARTII.Tests
{
    public class Condition2Config
    {
        static Condition2Config()
        {
            MSSQLDataSetting.DefaultScriptOptions = () =>
            {
                var dbAssembly = typeof(SMARTIIEntities).Assembly;
                var excutingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

                return ScriptOptions.Default
                                    .AddReferences(dbAssembly)
                                    .AddReferences(excutingAssembly)
                                    .WithImports(new string[] {
                                        "System",
                                        "System.Collections",
                                        "System.Collections.Generic",
                                        "System.Linq"
                                    });
            };
        }

        public static ISetup[] Setups = new ISetup[]
        {
            // 設定MSSQL CONDITION 的相關配置
            // 相關作法請參閱 :
            // http://tfs2:8080/tfs/SD4-Collection/LibrarySD4/_git/Condition2?path=%2FREADME_MSSQL.md&version=GBmaster&_a=preview
              new MSSQLDataSetting()
              {
                  // 回傳預設連線位址實體
                  DefaultDBContextDelegate = () => new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn),

                  // 回傳物件映射的設定檔
                  DefaultMappingConfig = () => AutoMapper.Mapper.Instance,
              }
        };
    }
}