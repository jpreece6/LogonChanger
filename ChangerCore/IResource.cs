using System;

namespace ChangerCore
{
    interface IResource
    {
        string GetResource(Uri remoteUri);
        string GetResource(string folderPath);

        string GetResourceFromConfig(string configPath);
    }
}
