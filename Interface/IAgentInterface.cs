using SarkPayOuts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Interface
{
   public  interface IAgentInterface
    {
        LoginViewModel LoginCheck(string userName,string password);
        bool RegisterAgent(RegistrationModel model);
        ProjectDetails   AddBlockedUnitsToDb(UnitModel model );
        List<ProjectDetails> GetAgentsUnits(string agentId);
    }
}
