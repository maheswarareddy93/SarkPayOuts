using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    public class CommonSetting
    {
        [Key]
        public int SettingID { get; set; }

        [StringLength(50)]
        public string SMTPServer { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Password { get; set; }

        public int Port { get; set; }

        [StringLength(100)]
        public string SiteURL { get; set; }

        public byte IsSSL { get; set; }

        [StringLength(200)]
        public string CompanyName { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
