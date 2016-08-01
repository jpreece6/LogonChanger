using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChangerCore.Exceptions
{
    public class UnsupportedOSException : Exception
    {
        public UnsupportedOSException(string message)
            : base(message)
        {
                
        }
    }
}
