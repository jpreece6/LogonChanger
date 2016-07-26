using SettingsVault;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LogonChanger
{
    class BingWebResource : WebResource
    {
        public override void GetResource(Uri remoteUri, string fileName)
        {
            var response = Connect(remoteUri);

            var bingHash = Util.GetMd5Hash(response);
            var localHash = Settings.Default.Get<string>(Config.BingHash, "");

            if (localHash.Equals(bingHash)) return;

            Settings.Default.Set(Config.BingHash, bingHash);
            Settings.Default.Save();

            ProcessXml(ref response);
            DownloadResource(new Uri("http://www.bing.com" + response + "_" + Config.Resolution + ".jpg"), fileName);
        }

        /// <summary>
        /// Read the XML data received from Bing
        /// </summary>
        /// <param name="res">url data returned as a string by reference</param>
        private void ProcessXml(ref string res)
        {
            using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(new StringReader(res)))
            {
                reader.ReadToFollowing(Config.BingXmlkey);
                res = reader.ReadElementContentAsString();
            }
        }
    }
}
