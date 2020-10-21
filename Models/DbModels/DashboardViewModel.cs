using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    public class DashboardViewModel
    {
        public int NewBookings { get; set; }
        public int TotalBookings { get; set; }
        public int TotalBlocked { get; set; }
        public int NewBlockedUnits { get; set; }
        public int Agents { get; set; }
    }
}
