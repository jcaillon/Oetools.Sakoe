using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Oetools.Runner.Cli.Core;
using Oetools.Runner.Cli.Resources.Dependencies;

namespace Oetools.Runner.Cli.Lib {
    internal static class AssemblyLoader {
        /// <summary>
        /// Called when the resolution of an assembly fails, gives us the opportunity to feed the required asssembly
        /// to the program
        /// Subscribe to the following event on start :
        /// AppDomain.CurrentDomain.AssemblyResolve += LibLoader.AssemblyResolver;
        /// </summary>
        public static Assembly AssemblyResolver(object sender, ResolveEventArgs args) {
            // see code https://msdn.microsoft.com/en-us/library/d4tc2453(v=vs.110).aspx
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);

            if (assembly != null) {
                return assembly;
            }

            var commaIdx = args.Name.IndexOf(",", StringComparison.CurrentCultureIgnoreCase);
            if (commaIdx > 0) {
                var assName = args.Name.Substring(0, commaIdx);

                return Assembly.Load(DependenciesResources.GetDependencyFromResources($"{assName}.dll"));
            }

            var assemblyName = new AssemblyName(args.Name).Name;
            var dllBytes = DependenciesResources.GetDependencyFromResources($"{assemblyName}.dll");
            var pdbBytes = DependenciesResources.GetDependencyFromResources($"{assemblyName}.pdb");

            if (dllBytes == null) {
                return null;
            }

            return pdbBytes != null ? Assembly.Load(dllBytes, pdbBytes) : Assembly.Load(dllBytes);
        }
    }
}