using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Common
{
    public static  class CommonMethods
    {
        public static string  GenerateuniqueId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 4); ;
        }

        public static string GenerateuniqueAgentId()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 9); ;
        }
    }
}
