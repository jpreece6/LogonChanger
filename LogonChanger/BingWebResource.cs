using SettingsVault;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LogonChanger
{
    class BingWebResource : WebResource
    {
        private string _resolution;
        private string _xmlKey;
        private string _url;

        public override string GetResourceFromConfig(string configPath)
        {
            GetConfigSettings();

            var fileName = Settings.Default.Get<string>(Config.WallpaperDir) + Util.GenerateFileTimeStamp() + ".jpg";
            var response = Connect(new Uri(_url));

            var bingHash = Util.GetMd5Hash(response);
            var localHash = Settings.Default.Get<string>(Config.BingHash, "");

            if (localHash.Equals(bingHash)) return "";

            Settings.Default.Set(Config.BingHash, bingHash);
            Settings.Default.Save();

            ProcessResponseXml(ref response);
            DownloadResource(new Uri("http://www.bing.com" + response + "_" + _resolution + ".jpg"), fileName);

            return fileName;
        }

        /// <summary>
        /// Read the XML data received from Bing
        /// </summary>
        /// <param name="response">url data returned as a string by reference</param>
        private void ProcessResponseXml(ref string response)
        {
            var xDoc = XDocument.Parse(response);

            var element = xDoc.Descendants(_xmlKey).FirstOrDefault();
            if (element != null) response = element.Value;

        }

        private void GetConfigSettings()
        {
            var xDoc = XDocument.Load(Config.RemoteConfigPath);

            var urlElem = xDoc.Root?.Element(Config.Url);
            if (urlElem != null) _url = urlElem.Value;

            var resolutionElem = xDoc.Root?.Element(Config.Resolution);
            if (resolutionElem != null) _resolution = resolutionElem.Value;

            var bingXmlkeyElem = xDoc.Root?.Element(Config.BingXmlkey);
            if (bingXmlkeyElem != null) _xmlKey = bingXmlkeyElem.Value;
        }
    }
}
