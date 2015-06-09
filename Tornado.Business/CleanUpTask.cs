using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Framework;

namespace Tornado.Business
{
    public class CleanUpTask
    {
        private readonly string[] _extensionsToAnalyse = { ".mkv", ".avi" };

        private readonly List<string> _failed = new List<string>();
        public bool IsRunning { get; private set; }

        public async Task CleanUp(string path)
        {
            try
            {
                IsRunning = true;
                _failed.Clear();

                IEnumerable<string> files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    try
                    {
                        await CleanUpFile(file);

                    }
                    catch (Exception)
                    {
                        _failed.Add(file);
                    }
                }
            }
            catch (Exception)
            {
                _failed.Add(path);
            }
            finally
            {
                IsRunning = false;
            }
        }

        private async Task CleanUpFile(string filename)
        {
            string extension = Path.GetExtension(filename);

            if (!CanAnalyse(extension))
                return;

            string hash = HashHelper.ComputeHash(filename);

            //Metadata [] metadatas = GetMetadata(hash);

        }



        private bool CanAnalyse(string extension)
        {
            return _extensionsToAnalyse.Any(k => string.Equals(k, extension, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
