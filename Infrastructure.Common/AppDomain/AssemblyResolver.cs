using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;

namespace Infrastructure.Common.AppDomain
{
    /// <summary>
    /// 用于为指定的应用程序域提供程序集解析器
    /// </summary>
    public class AssemblyResolver : MarshalByRefObject
    {
        /// <summary>
        /// 可能包含程序集文件的子目录
        /// </summary>
        private static readonly string[] AssemblyProbeSubdirectories =
        {
            string.Empty, "bin", "debug", "release", "amd64",
            "i386", @"..\..\..\..\..\private\lib"
        };

        /// <summary>
        /// 可能包含程序集文件的主目录
        /// </summary>
        private static Collection<string> _assebmlyProbeDirectories = new Collection<string>();

        /// <summary>
        /// 已被解析过的程序集
        /// </summary>
        private static Dictionary<string, Assembly> _resolvedAssemblies = new Dictionary<string, Assembly>();

        static AssemblyResolver()
        {
            // 初始化探测主目录
            InitProbeMainDirectory();
        }

        /// <summary>
        /// 附加解析器到当前应用程序域
        /// </summary>
        public void AttachResolver()
        {
            this.AttachResolver(System.AppDomain.CurrentDomain);
        }

        /// <summary>
        /// 附加解析器到指定应用程序域
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <exception cref="System.ArgumentNullException">domain</exception>
        public void AttachResolver(System.AppDomain domain)
        {
            if (null == domain)
            {
                throw new ArgumentNullException("domain");
            }

            domain.AssemblyResolve += this.ResolveAssembly;
        }

        /// <summary>
        /// 获取程序集文件位置.
        /// </summary>
        /// <param name="fileName">程序集文件名.</param>
        /// <returns>程序集文件位置</returns>
        public static string GetAssemblyLocation(string fileName)
        {
            // 获取所有可能的程序集路径
            IEnumerable<string> possibleAssemblyPaths = GetPossibleAssemblyPaths(fileName);

            // 循环探测目录来查找程序集是否存在
            foreach (string assemblyPath in possibleAssemblyPaths)
            {
                if (!File.Exists(assemblyPath))
                {
                    continue;
                }

                return assemblyPath;
            }

            return string.Empty;
        }

        /// <summary>
        /// 解析程序集
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns>Assembly</returns>
        private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            // 如果程序集已被解析过，则直接返回程序集
            Assembly resolvedAssembly;
            if (_resolvedAssemblies.TryGetValue(args.Name, out resolvedAssembly))
            {
                return resolvedAssembly;
            }

            AssemblyName name = new AssemblyName(args.Name);
            string fileName = name.Name + ".dll";

            // 获取程序集位置
            string assemblyLocation = GetAssemblyLocation(fileName);
            if (!string.IsNullOrEmpty(assemblyLocation))
            {
                try
                {
                    Assembly assembly = Assembly.Load(AssemblyName.GetAssemblyName(assemblyLocation));
                    _resolvedAssemblies[args.Name] = assembly;
                    return assembly;
                }
                catch (BadImageFormatException)
                {
                    // 忽略这个程序集
                }
            }

            _resolvedAssemblies[args.Name] = null;
            return null;
        }

        private static IEnumerable<string> GetPossibleAssemblyPaths(string fileName)
        {
            var assemblyPaths = new List<string>();

            foreach (string baseDirectory in _assebmlyProbeDirectories)
            {
                foreach (string subdirectory in AssemblyProbeSubdirectories)
                {
                    string assemblyPath = Path.Combine(Path.Combine(baseDirectory, subdirectory), fileName);

                    assemblyPaths.Add(assemblyPath);
                }
            }

            return assemblyPaths;
        }

        private static void InitProbeMainDirectory()
        {
            string baseDirectory = Path.GetDirectoryName(System.AppDomain.CurrentDomain.BaseDirectory);
            if (!string.IsNullOrEmpty(baseDirectory))
            {
                _assebmlyProbeDirectories.Add(baseDirectory.ToUpperInvariant());
            }

            string currentDirectory = Path.GetDirectoryName(Environment.CurrentDirectory);
            if (!string.IsNullOrEmpty(currentDirectory) && !_assebmlyProbeDirectories.Contains(currentDirectory))
            {
                _assebmlyProbeDirectories.Add(currentDirectory);
            }
        }
    }
}
