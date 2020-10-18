using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models
{
    public class BlockedUnits:UnitModel 
    {
        public string Id { get; set; }
        public string CreatedDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Reason { get; set; }
        public string StatusConfiredDate { get; set; }
    }
    public class ProjectDetails 
    {
        public string ProjectName { get; set; }
        public string ProjectId { get; set; }
        public List<BlockedUnits> UnitsData { get; set; }
    }

    public class MyBookinsViewModel
    {
        public List<ProjectDetails> lstBlocked { get; set; }
        public List<ProjectDetails> lstRejected { get; set; }
        public List<ProjectDetails> lstBooked { get; set; }
    }
}
