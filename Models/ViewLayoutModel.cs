using SarkPayOuts.Models.DbModels;
using System.Collections.Generic;

namespace SarkPayOuts.Models
{
    public class ViewLayoutModel : ProjectUnitsData
    {
        public string ProjectName { get; set; }
        public List<string> projects{get;set;}
    }

}
