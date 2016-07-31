using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using SettingsVault;

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

            if (_enumeratedFiles.Length == 0)
                return null;

            var shuffle = Settings.Default.Get<bool>(Config.Shuffle, false);

            if (shuffle)
            {

                Random rnd = new Random();
                int randIndex = rnd.Next(0, _enumeratedFiles.Length);

                return _enumeratedFiles[randIndex];

            }

            var fileIndex = Settings.Default.Get<int>(Config.FileIndex, 0);

            // If larger than our bounds reset to 0
            if (fileIndex > _enumeratedFiles.Length)
                fileIndex = 0;

            var selectedFile = _enumeratedFiles[fileIndex];

            Settings.Default.Set(Config.FileIndex, ++fileIndex);
            Settings.Default.Save();

            return selectedFile;
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
