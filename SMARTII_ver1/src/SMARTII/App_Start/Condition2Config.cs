using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting;
using Ptc.Data.Condition2.Interface.Common;
using Ptc.Data.Condition2.Mssql.Common;
using SMARTII.Database.SMARTII;
using SMARTII.Domain.Cache;

namespace SMARTII.App_Start
{
    public class Condition2Config
    {
        // 建立DLL 配置 :
        // 這邊是設定關於CONDITION2 引用 CSHARP SCRIIPT 的全域設定。
        static Condition2Config()
        {
            MSSQLDataSetting.DefaultScriptOptions = () =>
            {
                // 引用SMARTII.Database.dll
                // 透過動態組合 Expression lambda 時 , 需要使用相關類別
                var dbAssembly = typeof(SMARTIIEntities).Assembly;

                // 載入各Bu的 dll
                var buAssembly = GlobalizationCache.Instance.AssemblyDict.Values.ToArray();

                // 載入各Domain的 dll(用於商品和門市的進階查詢)
                var domainAssembly = Assembly.Load("SMARTII.Domain");

                // 取得目前正在執行的 .dll
                var excutingAssembly = System.Reflection.Assembly.GetExecutingAssembly();

                // 建立參考
                // 引用 BCL 函式庫 , 確保 compile 過程無誤。
                return ScriptOptions.Default
                                    .AddReferences(dbAssembly)
                                    .AddReferences(domainAssembly)
                                    .AddReferences(buAssembly)
                                    .AddReferences(excutingAssembly)
                                    .WithImports(new string[] {
                                        "System",
                                        "System.Collections",
                                        "System.Collections.Generic",
                                        "System.Linq"
                                    });
            };
        }

        /// <summary>
        /// 設定MSSQL CONDITION 的相關配置
        /// 相關作法請參閱 :
        /// http://tfs2:8080/tfs/SD4-Collection/LibrarySD4/_git/Condition2?path=%2FREADME_MSSQL.md&version=GBmaster&_a=preview
        /// </summary>
        public static ISetup[] Setups = new ISetup[]
        {
              // 針對MSSQL 進行初始值宣告
              new MSSQLDataSetting()
              {
                  // 回傳預設連線位址實體
                  DefaultDBContextDelegate = () => new SMARTIIEntities(DataAccessCache.Instance.SmartIIConn),

                  // 回傳物件映射的設定檔 (此專案使用Automapper v7.0.1)
                  // 此為靜態類別注射 , 設定方式請參閱 :
                  // SMARTII.Assist.Mapper.EntryProfile
                  DefaultMappingConfig = () => AutoMapper.Mapper.Instance,
              }
        };
    }
}
