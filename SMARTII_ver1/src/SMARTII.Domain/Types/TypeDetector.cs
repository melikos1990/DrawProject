using System;
using System.Linq;
using System.Reflection;
using System.Web;
using SMARTII.Domain.Cache;

namespace SMARTII.Domain.Types
{
    public static class TypeDetector
    {
        private static string GetServiceNameFromHttpHeader()
        {
            return HttpContext.Current.Request.Headers[EssentialCache.NodeDefinitionValue.BusinessUnit];
        }

        private static Type FindConcreteType(this Assembly assembly, Type type)
        {
            return assembly.GetTypes()
                           .First(p => p.IsInterface == false &&
                                       type.IsAssignableFrom(p));
        }

        private static Type FindConcreteType(this Assembly assembly, Type type, string name)
        {
            return assembly.GetTypes()
                           .First(p => p.IsInterface == false &&
                                       type.IsAssignableFrom(p) &&
                                       p.Name == name);
        }

        public static Type TryGetConcreteType(this Type type)
        {
            if (type.IsInterface == false || type.IsGenericType)
                return type;

            var serviceName = GetServiceNameFromHttpHeader();

            if (string.IsNullOrEmpty(serviceName))
                return type;

            if (GlobalizationCache
                .Instance
                .AssemblyDict
                .TryGetValue(serviceName, out Assembly serviceAssembly) == false)
                return type;

            var concrete = serviceAssembly.FindConcreteType(type);

            return concrete;
        }
    }
}