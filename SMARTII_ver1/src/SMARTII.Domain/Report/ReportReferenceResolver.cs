using System.Collections.Generic;
using System.IO;
using System.Linq;
using RazorEngine.Compilation;
using RazorEngine.Compilation.ReferenceResolver;

namespace SMARTII.Domain.IO
{
    internal class ReportReferenceResolver : IReferenceResolver
    {
        private string[] _assembliesToLoad;

        public ReportReferenceResolver(params string[] assembliesToLoad)
        {
            _assembliesToLoad = assembliesToLoad;
        }

        public IEnumerable<CompilerReference> GetReferences(TypeContext context, IEnumerable<CompilerReference> includeAssemblies = null)
        {
            IEnumerable<string> loadedAssemblies = CompilerServicesUtility
                .GetLoadedAssemblies()
                .Where(a => !a.IsDynamic && !a.FullName.Contains("Version=0.0.0.0") && File.Exists(a.Location) && !a.Location.Contains("CompiledRazorTemplates.Dynamic"))
                .GroupBy(a => a.GetName().Name).Select(grp => grp.First(y => y.GetName().Version == grp.Max(x => x.GetName().Version))) // only select distinct assemblies based on FullName to avoid loading duplicate assemblies
                .Select(a => CompilerReference.From(a))
                .Concat(includeAssemblies ?? Enumerable.Empty<CompilerReference>())
                .Select(r => r.GetFile())
                .ToArray();
            yield return CompilerReference.From(FindLoaded(loadedAssemblies, "mscorlib.dll"));
            yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.dll"));
            yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Web.dll"));
            yield return CompilerReference.From(FindLoaded(loadedAssemblies, "System.Core.dll"));
            yield return CompilerReference.From(FindLoaded(loadedAssemblies, "RazorEngine.dll"));
            yield return CompilerReference.From(FindLoaded(loadedAssemblies, "SMARTII.Resource.dll"));

            yield return CompilerReference.From(typeof(ReportReferenceResolver).Assembly); // Assembly
            foreach (var assembly in this._assembliesToLoad)
            {
                yield return CompilerReference.From(FindLoaded(loadedAssemblies, assembly));
            }
        }

        private string FindLoaded(IEnumerable<string> refs, string find)
        {
            return refs.First(r => r.EndsWith(Path.DirectorySeparatorChar + find));
        }
    }
}
