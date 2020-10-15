using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    [Table("AgentPayOuts")]
    public class AgentPayOuts
    {
        [Key]
        public string LeadUUID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string AgentId { get; set; }
        public string ProjectName { get; set; }
        public string UnitSize { get; set; }
        public string UnitNumber { get; set; }
        public decimal? PayoutPercentage { get; set; }
        public decimal? TdsPercentage { get; set; } 
        public decimal? GstPercentage { get; set; }
        public decimal? PayoutAmount { get; set; }
        public decimal? TdsAmount { get; set; }
        public decimal? GstAmount { get; set; }         
        public string PayOutStatus { get; set; }
        public string CreationDate { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string LeadHistory { get; set; }
    }
}
