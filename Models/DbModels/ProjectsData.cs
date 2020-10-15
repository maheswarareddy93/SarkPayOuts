using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models.DbModels
{
    [Table("ProjectsData")]
    public class ProjectsData
    {
        [Key]
        public int ProjectId { get; set; }
        [StringLength(70)]
        public string ProjectName { get; set; }
        [StringLength(20)]
        public string status { get; set; }
        public string projectuuid { get; set; }
    }
}
