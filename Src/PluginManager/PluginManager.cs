using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginManager
{
    public class PluginManager<T>
    {
        public List<Exception> Errors { get; private set; }

        public PluginManager()
        {
            Errors = new List<Exception>();
        }
        public List<PluginAssembly<T>> LoadPlugins(string path, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var dir = new DirectoryInfo(path);

            return LoadPlugins(dir, searchOption);
        }

        private List<PluginAssembly<T>> LoadPlugins(DirectoryInfo dir, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            List<PluginAssembly<T>> result = new List<PluginAssembly<T>>();

            List<Assembly> allAssemblies = new List<Assembly>(LoadAssemblies(dir, searchOption));
            foreach (var assembly in allAssemblies)
            {
                PluginAssembly<T> pluginAssembly = new PluginAssembly<T>();
                pluginAssembly.Assembly = assembly;

                foreach (var type in assembly.GetTypes().Where(p => typeof(T).IsAssignableFrom(p)))
                {
                    pluginAssembly.PluginClasses.Add(type);
                }
            }

            return result;
        }

        private List<Assembly> LoadAssemblies(DirectoryInfo dir, SearchOption searchOption)
        {
            List<Assembly> result = new List<Assembly>();
            var assemblyFiles = Directory.GetFiles(dir.FullName, "*.dll", searchOption);

            foreach (var assemblyFile in assemblyFiles)
            {
                try
                {
                    var assembly = Assembly.Load(assemblyFile);
                    result.Add(assembly);
                }
                catch (Exception ex)
                {
                    Errors.Add(ex);
                }
            }

            return result;
        }
    }

    public class PluginAssembly<T>
    {
        public PluginAssembly()
        {
            PluginClasses = new List<Type>();
        }
        public Assembly Assembly { get; set; }

        public List<Type> PluginClasses { get; }
    }
}
