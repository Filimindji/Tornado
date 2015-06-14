using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Tornado.Plugin;

namespace Tornado.Business
{
    public class PluginManager
    {

        public static TornadoPluginBase[] Plugins { get; private set; }

        public Task LoadAsync(string path)
        {
            return Task.Run(() => Load(path));
        }

        public void Load(string path)
        {
            List<TornadoPluginBase> plugins = new List<TornadoPluginBase>();

            DirectoryInfo dir = new DirectoryInfo(path);

            foreach (FileInfo file in dir.GetFiles("Plugins\\Tornado.Plugin.*.dll", SearchOption.AllDirectories))
            {
                Assembly assembly = Assembly.LoadFrom(file.FullName);
                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsSubclassOf(typeof(TornadoPluginBase)) || type.IsAbstract)
                        continue;

                    TornadoPluginBase plugin = type.InvokeMember(null,
                        BindingFlags.CreateInstance,
                        null, null, null) as TornadoPluginBase;

                    plugins.Add(plugin);
                }
            }

            Plugins = plugins.ToArray();
        }
    }
}