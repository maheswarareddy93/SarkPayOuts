using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Models
{
    public class AgentModel
    {
        public string  AgentId { get; set; }
        public string  AgentName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }

        public string Aadhar { get; set; }

        public string Pan { get; set; }

        public bool IsActive { get; set; }

        public string Password { get; set; }

        
    }
    public class AgentViewModel
    { 
      public List<AgentModel> AgentsList { get; set; }
     public RegistrationModel Registration { get; set; }
    }
}
