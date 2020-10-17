using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    public class AutharityData
    {
        [Key]
        public int Id { get; set; }

        [StringLength(45)]
        public string LeadsMailTo { get; set; }

        [StringLength(45)]
        public string LeadsMailCc { get; set; }

        public string AdminMobile { get; set; }

        public string SuperAdminMailId { get; set; }

        public string AccountMailId { get; set; }

        public string AccountMobile { get; set; }

        public string AdminMailId { get; set; }
    }
}
