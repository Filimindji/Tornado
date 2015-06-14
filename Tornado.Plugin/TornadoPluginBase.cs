using System.Reflection;

namespace Tornado.Plugin
{
    public class Metadata
    {
        public Metadata(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; set; }
        public string Value { get; set; }
    }

    public abstract class TornadoPluginBase
    {
        public abstract Metadata[] GetMetadata(string filename);
    }
}
