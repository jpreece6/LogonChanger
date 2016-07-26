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
    class WebResource
    {
        protected virtual string Connect(Uri remoteUri, string requestMethod = "GET")
        {
            // Create a new GET request for the Bing XML data file
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(remoteUri.OriginalString);
            request.KeepAlive = false;
            //request.Method = "GET";
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
                client.DownloadFile(remoteUri.OriginalString, fileName);
            }
        }

        public virtual void GetResource(Uri remoteUri, string fileName)
        {
            DownloadResource(remoteUri, fileName);
        }
    }
}
