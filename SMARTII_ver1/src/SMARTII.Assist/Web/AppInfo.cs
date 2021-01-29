using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using SMARTII.Assist.Culture;
using SMARTII.Assist.Logger;

namespace SMARTII.Assist.Web
{
    public static class AppInfo
    {
        public static Dictionary<string, string> GetControllerDict(this Assembly[] assemblies)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var assem in assemblies)
            {
                var controllerTypes = assem.GetExportedTypes()
                                     .Where(x => x.Name.Contains("Controller"));

                foreach (var type in controllerTypes)
                {
                    var methods = type.GetMethods();

                    methods.ToList().ForEach(x =>
                    {
                        var attr = x.GetCustomAttribute<LoggerAttribute>();

                        if (attr == null) return;

                        if (dict.TryGetValue(attr.FeatureTag, out string v) == false)
                        {
                            var name = attr.FeatureTag.GetSpecificLang(new CultureInfo("zh-TW", false));

                            dict.Add(attr.FeatureTag, name);
                        }
                    });
                }
            }
            return dict;
        }
    }
}