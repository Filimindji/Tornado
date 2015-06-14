using System.IO;
using ServiceStack;
using Tornado.Server.ServiceModel;

namespace Tornado.Business
{
    public class RenameRule : Rule
    {
        public string FolderDestination { get; set; }
        public string FilenameFormat { get; set; }
        protected override void DoExecute(string filename, FileResponse file)
        {
            if (FolderDestination == null)
                FolderDestination = Path.GetDirectoryName(filename);

            string newName = FilenameFormat.Fmt();
            string newPath = Path.Combine(FolderDestination, newName);

            if (System.IO.File.Exists(newPath) == false)
                System.IO.File.Move(filename, newPath);
        }
    }
}