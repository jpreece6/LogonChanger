using System;
using System.IO;

namespace ChangerCore
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
