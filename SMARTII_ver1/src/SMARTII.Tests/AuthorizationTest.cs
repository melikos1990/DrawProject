using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SMARTII.Assist.Culture;
using SMARTII.Assist.Logger;
using SMARTII.Domain.Authentication.Object;
using SMARTII.Domain.Authentication.Service;
using SMARTII.Domain.Organization;

namespace SMARTII.Tests
{
    [TestClass]
    public class AuthorizationTest
    {
        public AuthorizationTest()
        {
        }

        [TestMethod]
        public void Test_Create_Token()
        {
            var user = new User()
            {
                Account = "ptc12876266",
                Name = "ptc"
            };

            var token = DIBuilder.Resolve<ITokenManager>().Create(user, null);

            var tuser = DIBuilder.Resolve<ITokenManager>().Parse<User>(token);
        }

        [TestMethod]
        public void Test_AD_Valid()
        {
            var helper = new LDAPHelper();

            helper.SetUp("SMARTII_SE", "pTC12876266");

            var test = helper.FindOne("rshov0123");

            //var isAdUser = helper.IsADUser("rshov0123");

            //List<ADUser> userList = helper.FindAll(string.Empty);
            //var s = userList.FirstOrDefault(x => x.userprincipalname == "rshov0123");
        }

        [TestMethod]
        public void Test_Get_Features()
        {
            Assembly[] assemblies = new Assembly[]
            {
              Assembly.Load("SMARTII"),
              Assembly.Load("SMARTII.ASO"),
              Assembly.Load("SMARTII.21Century"),
            };

            Dictionary<string, string> dict = new Dictionary<string, string>();

            var s = assemblies.FirstOrDefault().GetExportedTypes().Where(x => x.Name.Contains("Controller"));

            foreach (var assem in assemblies)
            {
                var ctrlTypes = assem.GetExportedTypes().Where(x => x.Name.Contains("Controller"));

                foreach (var type in ctrlTypes)
                {
                    var methods = type.GetMethods();

                    methods.ToList().ForEach(x =>
                    {
                        var attr = x.GetCustomAttribute<LoggerAttribute>();

                        if (attr == null) return;

                        var ch = attr.FeatureTag.GetSpecificLang(new CultureInfo("zh-TW", false));

                        dict.Add(ch, attr.FeatureTag);
                    });
                }
            }
        }
    }
}
