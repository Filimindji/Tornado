using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using Tornado.Framework;
using Tornado.Server.ServiceModel;
using File = Tornado.Server.ServiceModel.File;

namespace Tornado.Business
{
    public class CleanUpTask
    {
        private readonly string[] _extensionsToAnalyse = { ".mkv", ".avi" };

        private readonly List<string> _failed = new List<string>();
        private string baseUri = "http://tornado-west-eu.cloudapp.net";
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

            FileResponse fileResponse = await GetFile(hash);
            fileResponse.Metadata = new[]
            {
                new Metadata() {Key = "extension", Value = Path.GetExtension(filename) }, 
            };

            RenameRule renameRule = new RenameRule();
            renameRule.Condition = "in([extension], '.mkv', '.avi')";
            renameRule.FilenameFormat = Path.GetFileName(filename);
            renameRule.Execute(filename, fileResponse);

        }

        private async Task<FileResponse> GetFile(string hash)
        {
            using (JsonServiceClient client = new JsonServiceClient(baseUri))
            {
                File fileRequest = new File();
                fileRequest.Hash = hash;
                return await client.PostAsync<FileResponse>(fileRequest);
            }
        }


        private bool CanAnalyse(string extension)
        {
            return _extensionsToAnalyse.Any(k => string.Equals(k, extension, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
