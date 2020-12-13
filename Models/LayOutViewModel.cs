
using System.Collections.Generic;

namespace SarkPayOuts.Models
{
    public class LayOutViewModel
    {
        public string Name { get; set; }
        public int AvailableUnits { get; set; }
        public int BolckedUnits { get; set; }
        public int BookedUnits { get; set; }
        public string ProjectId { get; set; }
        public string Type { get; set; }
        public List<AgentModel> lstAgents { get; set; }
    }
}
