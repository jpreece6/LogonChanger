using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogonChanger
{
    class LocalResource : IResource
    {

        private string[] _enumeratedFiles;

        public string GetResource(Uri remoteUri)
        {
            throw new NotImplementedException();
        }

        public string GetResource(string folderPath)
        {
            EnumerateFiles(folderPath);
            Random rnd = new Random();
            int randIndex = rnd.Next(0, _enumeratedFiles.Length);

            return _enumeratedFiles[randIndex];
        }

        public string GetResourceFromConfig(string configPath)
        {

            throw new NotImplementedException();
        }

        private void EnumerateFiles(string folderPath)
        {
            if (Directory.Exists(folderPath))
                _enumeratedFiles = Directory.GetFiles(folderPath);
        }
    }
}
