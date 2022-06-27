using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System_Information.Core
{
    public class ConvertorUtils
    {

        public UInt64 BytsToGigaBytes(UInt64 bytes)
        {
            return bytes / 1073741824;
        }

        public UInt64 BytesToMegaBytes(UInt64 bytes)
        {
            return bytes / 1024 / 1000;
        }
    }
}
