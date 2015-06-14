using System.Collections.Generic;
using System.Linq;

namespace Tornado.Server.ServiceModel
{
    public partial class FileResponse
    {
        private Dictionary<string, List<string>> _metadataDictionary;

        public Dictionary<string, List<string>> Metadatas
        {
            get
            {
                if (_metadataDictionary == null)
                    _metadataDictionary =
                        (from k in Metadata
                            group k by k.Key
                            into g
                            select g).ToDictionary(k => k.Key, k => k.Select(l => l.Value).ToList());

                return _metadataDictionary;
            }
        }
    }
}