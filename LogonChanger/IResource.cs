using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogonChanger
{
    interface IResource
    {
        bool GetResource(Uri remoteUri, string fileName);
        bool GetResource(string folderPath);
    }
}
