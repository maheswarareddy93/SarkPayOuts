using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    public class RejectedUnits:BlockedUnits 
    {
        public string Reason { get; set; }
    }
}
