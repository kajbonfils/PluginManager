using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PluginInterface;
using PluginManager;

namespace PluginLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "C:\\temp\\plugins";

            var pm = new PluginManager<IPlugin>();
            var pluginClasses = pm.LoadPlugins(path);

            foreach (var c in pluginClasses)
            {
                Console.WriteLine(c.Assembly.FullName);

                foreach (var cc in c.PluginClasses)
                {
                    Console.WriteLine(cc.FullName);
                    var p = cc.Instantiate<IPlugin>();
                    Console.WriteLine(p.Name);
                }
            }

            Console.ReadLine();
        }
    }

    public static class TypeExtension
    {
        public static T Instantiate<T>(this Type t)
        {
            return (T) Activator.CreateInstance(t);
        }
    }
}
