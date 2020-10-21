using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
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
        MyBookinsViewModel GetAgentsUnits(string agentId,string type);
       AgentRegistration CheckAdminExitsSendPassword(string id);
        public DashboardViewModel DashboardData(string id);
    }
}
