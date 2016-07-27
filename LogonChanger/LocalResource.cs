using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogonChanger
{
    class LocalResource : IResource
    {
        public bool GetResource(Uri remoteUri, string fileName)
        {
            throw new NotImplementedException();
        }

        public bool GetResource(string folderPath)
        {
            throw new NotImplementedException();
        }
    }
}
