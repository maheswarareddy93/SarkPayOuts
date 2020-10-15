using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    [Table("AgentRegistration")]
     public class AgentRegistration
    {
        [Key]
        public string AgentId { get; set; }
        public string AgetName { get; set; } 
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string PAN { get; set; }
        public string Aadhar { get; set; }
        public string ParentAgentID { get; set; }
        public string AccountHolderName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public bool? IsActive { get; set; }
        public string Docuents { get; set; }

        public string CreatedDate { get; set; }
        public string Password { get; set; }
        public string BlockedUnits { get; set; }
        public string BookingConfirmed { get; set; }
        public string RejectedUnits { get; set; }
    }
}

