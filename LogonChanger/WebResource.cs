using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SettingsVault;

namespace LogonChanger
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

        protected virtual void DownloadResource(Uri remoteUri, string fileName)
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
                        return;
                    }
                }
            }

            Logger.WriteInformation("Wallpaper downloaded successfully and saved to: " + fileName);
        }

        public virtual bool GetResource(Uri remoteUri, string fileName)
        {
            DownloadResource(remoteUri, fileName);

            return File.Exists(fileName);
        }

        public bool GetResource(string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
