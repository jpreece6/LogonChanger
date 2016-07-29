using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogonChanger
{
    interface IResource
    {
        string GetResource(Uri remoteUri);
        string GetResource(string folderPath);

        string GetResourceFromConfig(string configPath);
    }
}
