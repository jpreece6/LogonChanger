using System;
using System.IO;
using System.Net;
using SettingsVault;

namespace ChangerCore
{
    class WebResource : IResource
    {
        protected virtual string Connect(Uri remoteUri, string requestMethod = "GET")
        {
            // Create a new GET request for the Bing XML data file
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(remoteUri.OriginalString);
            request.KeepAlive = false;
            //request.Method = "GET";
            request.Method = requestMethod;
            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        protected virtual bool DownloadResource(Uri remoteUri, string fileName)
        {
            using (var client = new WebClient())
            {
                try
                {
                    client.DownloadFile(remoteUri.OriginalString, fileName);
                }
                catch (Exception ex)
                {
                    if (ex is ArgumentException || ex is WebException || ex is NotSupportedException)
                    {
                        Logger.WriteError("Failed to download resource", ex);
                        return false;
                    }
                }
            }

            if (!File.Exists(fileName))
            {
                Logger.WriteError("File downloaded but was not found!");
                return false;
            }

            Logger.WriteInformation("Wallpaper downloaded successfully and saved to: " + fileName);
            return true;
        }

        public virtual string GetResource(Uri remoteUri)
        {
            var fileName = Settings.Default.Get<string>(Config.WallpaperDir) + Util.GenerateFileTimeStamp() + ".jpg";

            DownloadResource(remoteUri, fileName);

            return fileName;
        }

        public virtual string GetResource(string folderPath)
        {
            throw new NotImplementedException();
        }

        public virtual string GetResourceFromConfig(string configPath)
        {
            throw new NotImplementedException();
        }
    }
}
