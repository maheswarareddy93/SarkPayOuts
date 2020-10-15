using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models
{
    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public bool IsActive { get; set; }
        public string Mobile { get; set; }

        public string  AdminId { get; set; }
    }
}
