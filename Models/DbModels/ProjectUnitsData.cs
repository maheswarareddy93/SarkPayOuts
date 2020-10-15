using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SarkPayOuts.Models.DbModels
{       
    [Table("ProjectUnitsData")]
    public class ProjectUnitsData
    {
        [Key]
        public int UnitId { get; set; }
        [StringLength(50)]
        public string UnitSize { get; set; }
        [StringLength(50)]
        public string UnitNumber { get; set; }
        public string Projectuuid { get; set; }
        [StringLength(20)]
        public string status { get; set; }
        public string BookingHistory { get; set; }
        public string  AgentId { get; set; }
        public string PaymentStatus { get; set; }
        [StringLength(60)]
        public string Facing { get; set; }
        [StringLength(60)]
        public string Mortigaze { get; set; }
        [StringLength(60)]
        public string BlockedDate { get; set; }
        [StringLength(60)]
        public string ExpiredDate { get; set; }
        
    }
}
