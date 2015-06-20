using System.Collections.Generic;

namespace Tornado.Plugin
{
    public class Metadata
    {
        public Metadata(string key, string value, int group = 0)
        {
            Key = key;
            Value = value;
            Group = group;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public int Group { get; set; }
    }

    public abstract class TornadoPluginBase
    {
        public abstract IEnumerable<Metadata> GetMetadata(string filename);
    }
}
