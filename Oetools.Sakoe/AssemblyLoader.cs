using System;
using System.IO;
using System.Reflection;
using System.Linq;

namespace Oetools.Sakoe {
        internal static class AssemblyLoader {

        private static string _executingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        
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

                return Assembly.Load(GetDependencyFromResources($"{assName}.dll"));
            }

            var assemblyName = new AssemblyName(args.Name).Name;
            var dllBytes = GetDependencyFromResources($"{assemblyName}.dll");
            var pdbBytes = GetDependencyFromResources($"{assemblyName}.pdb");

            if (dllBytes == null) {
                return null;
            }

            return pdbBytes != null ? Assembly.Load(dllBytes, pdbBytes) : Assembly.Load(dllBytes);
        }
        
        private static byte[] GetBytesFromResource(string resourcePath) {
            using (Stream resFilestream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcePath)) {
                if (resFilestream == null) {
                    return null;
                }

                var output = new byte[resFilestream.Length];
                resFilestream.Read(output, 0, output.Length);
                return output;
            }
        }

        private static byte[] GetDependencyFromResources(string fileName) {
            return GetBytesFromResource($"{_executingAssemblyName}.{fileName}");
        }
    }
}