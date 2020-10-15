using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Interface
{
    public interface IAdminOperations
    {
        void PopulateUnitsDataToDb(List<ProjectUnitsData> data);
        string  AddProjectToDB(ProjectsData data);

        LoginViewModel AdminLoginCheck(string userName, string password);

        List<AgentModel> GetAgentsData();
        bool CheckAgentExists(string phone,string email);
        bool RegisterNewAgent(RegistrationModel model);
        bool UpdatingBlockingUnitsStatus();
        ProjectDetails AddBlockedUnitsToDb(UnitModel model);
        List<NewBookingViewModel> GetNewBookings();
    }
}
