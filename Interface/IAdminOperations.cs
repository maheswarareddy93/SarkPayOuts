﻿using SarkPayOuts.Models;
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
        BookingNewViewModel  GetNewBookings(string id);

        AdminDetails CheckAdminExitsSendPassword(string id);
        public bool UpdateStatusOfBooking(string agentId,string projectId,string UnitNumber,string status,string type,string nae,string adid);
        public bool UpdateActiveStatus(string agentId,bool status);
        public bool DeleteAgentFromDb(string agentId);
        public DashboardViewModel   DashboardData(string Id);

        public List<ViewLayoutModel> GetAllUnitsData(string id);
        public List<ProjectsData> GetProjectsData();
        public AdminDetails GetAdminDetails(string  id);
    }
}
