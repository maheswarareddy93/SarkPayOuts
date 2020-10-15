using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    [Table("AdminDetails")]
    public class AdminDetails
    {
        [Key]
        public int AdminId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public bool IsActive { get; set; }
        [StringLength(10)]
        public string password { get; set; }
        public string BlockedUnits { get; set; }
        public string BookingConfirmed { get; set; }
        public string RejectedUnits { get; set; }
        public string AdminUUID { get; set; }
    }
}
