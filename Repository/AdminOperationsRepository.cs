using Microsoft.AspNetCore.Http;
using SarkPayOuts.Common;
using SarkPayOuts.Enums;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Transactions;

namespace SarkPayOuts.Repository
{
    public class AdminOperationsRepository: IAdminOperations
    {
        private readonly ApplicationDBContext _context;
        public AdminOperationsRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public string  AddProjectToDB(ProjectsData data)
        {
            string Id = string.Empty;
            if (data!=null) {
                ProjectsData details = _context.ProjectsData.Where(x=>x.ProjectName ==data.ProjectName).FirstOrDefault();
                if (details != null)
                {
                    Id = details.projectuuid;
                    return Id;
                }
                else {
                    Id= CommonMethods.GenerateuniqueId();
                    data.projectuuid = Id;
                    _context.ProjectsData.Add(data);
                    _context.SaveChanges();
                }
                return Id;
            }
            return null;
        }

        public LoginViewModel AdminLoginCheck(string userName, string password)
        {
            LoginViewModel model = new LoginViewModel();
            var loginDetails = _context.AdminDetails.
                Where(x => x.Email == userName && x.password == password && x.IsActive == true).FirstOrDefault();
            if (loginDetails != null)
            {
                model.AgentName = loginDetails.Name ;
                model.UserName = loginDetails.Email;
                model.Password = loginDetails.password;
                model.IsActive = loginDetails.IsActive;
                model.AdminId = loginDetails.AdminUUID;
                model.Mobile = loginDetails.Mobile;
                model.AgentName = loginDetails.Name;
                return model;
            }
            return null;
        }
        //Excel data populating into database
        public void PopulateUnitsDataToDb(List<ProjectUnitsData> model)
        {
            try
            {
                if (model.Count > 0)
                {
                    foreach (ProjectUnitsData  list in model)
                    {
                        using (var scope = new TransactionScope())
                        {
                            if (list.UnitNumber != "Unit" && list.UnitSize != "area")
                            {
                                _context.ProjectUnitsData.Add(list);
                                _context.SaveChanges();
                                scope.Complete();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<AgentModel> GetAgentsData()
        {
            List<AgentModel> lstAgents = new List<AgentModel>();
            var agentsData = _context.AgentRegistration.ToList();
            if (agentsData!=null && agentsData.Count>0)
            {
                foreach (var agentData in agentsData)
                {
                    AgentModel model = new AgentModel();
                    model.AgentId = agentData.AgentId;
                    model.AgentName = agentData.AgetName;
                    model.Email = agentData.Email;
                    model.Mobile = agentData.Mobile;
                    model.Aadhar = agentData.Aadhar;
                    model.Pan = agentData.PAN;
                    model.Password  = agentData.Password;
                    model.IsActive = (bool)agentData.IsActive;
                    lstAgents.Add(model);
                }
            }
            return lstAgents;
        }

        public bool CheckAgentExists(string phone, string email)
        {
            var agentDetails=_context.AgentRegistration.Where(x => x.Mobile == phone && x.Email == email).FirstOrDefault();
            if (agentDetails==null) {
                return false  ;
            }
            return true  ;
        }
        //Register New Agents 
        public bool RegisterNewAgent(RegistrationModel model)
        {
            try
            {
                if (model!=null)
                {
                    AgentRegistration reg = new AgentRegistration();
                    reg.AgetName = model.AgentName;
                    reg.Mobile = model.Mobile;
                    reg.Email = model.Email;
                    reg.Aadhar = model.Aaadhar;
                    reg.PAN = model.PAN;
                    reg.AgentId  = CommonMethods.GenerateuniqueAgentId();
                    reg.Password  = model.Password;
                    reg.AccountHolderName = model.AccountHolderName;
                    reg.BankAccountNumber = model.BankAccountNumber;
                    reg.IFSCCode = model.IFSCCode;
                    reg.CreatedDate = DateTime.Now.ToString("dd-MM-yyyy");
                    reg.IsActive = false;
                    _context.AgentRegistration.Add(reg);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
        //
        public bool UpdatingBlockingUnitsStatus()
        {
            try
            {
              var agents=  _context.AgentRegistration.ToList();
                if (agents!=null && agents.Count>0)
                {
                    foreach (var agent in agents)
                    {
                        if (!string.IsNullOrEmpty(agent.BlockedUnits))
                        {
                            List<ProjectDetails> lstNewList = new List<ProjectDetails>();
                            List<BlockedUnits> lstNewBlockedUnits = new List<BlockedUnits>();
                            AgentRegistration agentUpdate = _context.AgentRegistration.Where(x => x.AgentId == agent.AgentId).FirstOrDefault();
                            List<ProjectDetails> lstDetails = JsonSerializer.Deserialize<List<ProjectDetails>>(agent.BlockedUnits);
                            List<ProjectDetails> lstNewDetails = lstDetails;
                            List<ProjectDetails> lstRejectedUnits = new List<ProjectDetails>();
                            ProjectDetails Newdetails = new ProjectDetails();
                            BlockedUnits rejected = new BlockedUnits();
                            if (!string.IsNullOrEmpty(agent.RejectedUnits)) {
                                lstRejectedUnits = JsonSerializer.Deserialize<List<ProjectDetails>>(agent.RejectedUnits);
                            }
                            if (lstDetails!=null && lstDetails.Count>0)
                            {
                                foreach (var project  in lstDetails)
                                {
                                    ProjectDetails rejectedUnits = new ProjectDetails();
                                    List<BlockedUnits> lstRejected = new List<BlockedUnits>();
                                    foreach (var unitdata in project.UnitsData)
                                    {
                                        DateTime expiryDate = DateTime.Parse(unitdata.ExpiryDate);
                                        if (DateTime.Now>expiryDate)
                                        {
                                            rejected.Id = unitdata.Id;
                                            rejected.Status  =StatusEnum.Rejected.ToString();
                                            rejected.UnitNumber = unitdata.UnitNumber;
                                            rejected.UnitSize = unitdata.UnitSize;
                                            rejected.Facing = unitdata.Facing;
                                            rejected.CreatedDate = unitdata.CreatedDate;
                                            rejected.ExpiryDate = unitdata.ExpiryDate;
                                            lstRejected.Add(rejected);
                                            ProjectUnitsData unitsData = _context.ProjectUnitsData.Where(x=>x.Projectuuid==project.ProjectId && x.UnitNumber == unitdata.UnitNumber).FirstOrDefault();
                                            unitsData.status = StatusEnum.Available.ToString();
                                            unitsData.AgentId = "";
                                            unitsData.BlockedDate = "";
                                            unitsData.ExpiredDate = "";
                                            rejectedUnits.ProjectId = project.ProjectId;
                                            rejectedUnits.ProjectName = project.ProjectName;
                                            if (!string.IsNullOrEmpty(agentUpdate.RejectedUnits))
                                            {                                                                                                                                                                          
                                                if (lstRejectedUnits != null)
                                                {
                                                    foreach (var data in lstRejectedUnits)
                                                    {
                                                        data.UnitsData.Add(rejected);
                                                    }
                                                }
                                                else
                                                {
                                                    lstRejectedUnits.Add(rejectedUnits);
                                                }
                                            }
                                            else
                                            {
                                                rejectedUnits.UnitsData = lstRejected;
                                                lstRejectedUnits.Add(rejectedUnits);
                                            }
                                            Newdetails = project;
                                            //UpdateProjectUnitStatus(agent, project, project.ProjectId, unitdata.UnitNumber);
                                        }
                                        else
                                        {
                                        }
                                    }
                                }
                                Newdetails.UnitsData.Remove(rejected);
                                lstDetails.Remove(Newdetails);
                                agentUpdate.RejectedUnits = JsonSerializer.Serialize(lstRejectedUnits);
                                agentUpdate.BlockedUnits = JsonSerializer.Serialize(lstDetails);
                                _context.SaveChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
        //
        public bool   UpdateProjectUnitStatus(AgentRegistration  agentDetails,ProjectDetails project,string projectId,string unitId) {

            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public ProjectDetails AddBlockedUnitsToDb(UnitModel model)
        {
            ProjectDetails projectDetails = new ProjectDetails();
            BlockedUnits units = new BlockedUnits();
            List<BlockedUnits> lstunits = new List<BlockedUnits>();
            List<ProjectDetails> lstDetails = new List<ProjectDetails>();
            List<ProjectDetails> newlstDetails = new List<ProjectDetails>();
            units.Id = CommonMethods.GenerateuniqueId();
            units.UnitNumber = model.UnitNumber;
            units.UnitSize = model.UnitSize;
            units.Facing = model.Facing;
            units.CreatedDate = DateTime.Now.ToString("MM-dd-yyyy hh:mm tt");
            units.ExpiryDate = DateTime.Now.AddDays(2).ToString("MM-dd-yyyy hh:mm tt");
            units.AgentId = model.AgentId;
            units.Status = StatusEnum.Blocked.ToString();
            units.ProjectName = model.ProjectName;
            projectDetails.ProjectName = model.ProjectName;
            lstunits.Add(units);
            projectDetails.UnitsData = lstunits;
            projectDetails.ProjectId = model.ProjectId;
            var unitDetails = _context.AdminDetails.Where(x => x.AdminUUID == model.AgentId).FirstOrDefault();
            var unitDetailsStatus = _context.ProjectUnitsData.Where(x => x.UnitNumber == model.UnitNumber && x.Projectuuid == model.ProjectId).FirstOrDefault();
            if (unitDetailsStatus != null || unitDetailsStatus.status == StatusEnum.Available.ToString())
            {
                unitDetailsStatus.status = StatusEnum.Blocked.ToString();
                unitDetailsStatus.BlockedDate = units.CreatedDate;
                unitDetailsStatus.ExpiredDate = units.ExpiryDate;
                unitDetailsStatus.AgentId = model.AgentId;
            }

            if (unitDetails != null)
            {
                if (!string.IsNullOrEmpty(unitDetails.BlockedUnits))
                {
                    List<ProjectDetails> unitsData = JsonSerializer.Deserialize<List<ProjectDetails>>(unitDetails.BlockedUnits);
                    lstDetails = unitsData;
                    unitsData = unitsData.Where(x => x.ProjectId == model.ProjectId).ToList();
                    if (unitsData != null && unitsData.Count > 0)
                    {
                        foreach (var data in unitsData)
                        {
                            data.UnitsData.Add(units);
                        }
                        newlstDetails = lstDetails;
                        unitDetails.BlockedUnits = JsonSerializer.Serialize(newlstDetails);
                    }
                    else
                    {
                        lstDetails.Add(projectDetails);
                        unitDetails.BlockedUnits = JsonSerializer.Serialize(lstDetails);

                    }
                }
                else
                {
                    lstDetails.Add(projectDetails);
                    unitDetails.BlockedUnits = JsonSerializer.Serialize(lstDetails);
                }
                _context.SaveChanges();
                return projectDetails;
            }
            return null;
        }
        //Getting the NewBookings Data 
        public List<NewBookingViewModel> GetNewBookings(string id)
        {
            List<NewBookingViewModel> lst = new List<NewBookingViewModel>();
            List<ProjectDetails> lstProjectDetails = new List<ProjectDetails>();
            List<ProjectDetails> lstRejProjectDetails = new List<ProjectDetails>();
            List<ProjectDetails> lstbookedProjectDetails = new List<ProjectDetails>();
            List<ProjectDetails> lstSelfProjectDetails = new List<ProjectDetails>();
            List<ProjectDetails> lstadbookedProjectDetails = new List<ProjectDetails>();
            List<ProjectDetails> lstRejectesProjectDetails = new List<ProjectDetails>();
            var admins = _context.AdminDetails.Where(x=>x.AdminUUID==id).FirstOrDefault();
            if (admins !=null)
            {
                if (!string.IsNullOrEmpty(admins.BlockedUnits))
                {
                    lstSelfProjectDetails = JsonSerializer.Deserialize<List<ProjectDetails>>(admins.BlockedUnits);
                    foreach (var projectData in lstSelfProjectDetails)
                    {
                        foreach (var unit in projectData.UnitsData)
                        {
                            NewBookingViewModel agentDetails = new NewBookingViewModel();
                            agentDetails.AgentId = admins .AdminUUID;
                            agentDetails.AgentName = "Self";
                            agentDetails.UnitSize = unit.UnitSize;
                            agentDetails.CreatedDate = unit.CreatedDate;
                            agentDetails.UnitNumber = unit.UnitNumber;
                            agentDetails.Facing = unit.Facing;
                            agentDetails.ProjectId = projectData.ProjectId;
                            agentDetails.ProjectName = projectData.ProjectName;
                            agentDetails.Status = unit.Status;
                            lst.Add(agentDetails);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(admins.RejectedUnits))
                {
                    lstRejectesProjectDetails = JsonSerializer.Deserialize<List<ProjectDetails>>(admins.RejectedUnits);
                    foreach (var projectData in lstRejectesProjectDetails)
                    {
                        foreach (var unit in projectData.UnitsData)
                        {
                            NewBookingViewModel agentDetails = new NewBookingViewModel();
                            agentDetails.AgentId = admins.AdminUUID;
                            agentDetails.AgentName = "Self";
                            agentDetails.UnitSize = unit.UnitSize;
                            agentDetails.CreatedDate = unit.CreatedDate;
                            agentDetails.UnitNumber = unit.UnitNumber;
                            agentDetails.Facing = unit.Facing;
                            agentDetails.ProjectId = projectData.ProjectId;
                            agentDetails.ProjectName = projectData.ProjectName;
                            agentDetails.Status = unit.Status;
                            lst.Add(agentDetails);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(admins.BookingConfirmed))
                {
                    lstadbookedProjectDetails = JsonSerializer.Deserialize<List<ProjectDetails>>(admins.BookingConfirmed);
                    foreach (var projectData in lstadbookedProjectDetails)
                    {
                        foreach (var unit in projectData.UnitsData)
                        {
                            NewBookingViewModel agentDetails = new NewBookingViewModel();
                            agentDetails.AgentId = admins.AdminUUID;
                            agentDetails.AgentName = "Self";
                            agentDetails.UnitSize = unit.UnitSize;
                            agentDetails.CreatedDate = unit.CreatedDate;
                            agentDetails.UnitNumber = unit.UnitNumber;
                            agentDetails.Facing = unit.Facing;
                            agentDetails.ProjectId = projectData.ProjectId;
                            agentDetails.ProjectName = projectData.ProjectName;
                            agentDetails.Status = unit.Status;
                            lst.Add(agentDetails);
                        }
                    }
                }
            }
            var agents =_context.AgentRegistration.ToList();
            foreach (var agent in agents)
            {
                if (!string.IsNullOrEmpty(agent.BlockedUnits))
                {                    
                    lstProjectDetails = JsonSerializer.Deserialize<List<ProjectDetails>>(agent.BlockedUnits);
                    foreach (var projectData in lstProjectDetails )
                    {
                        foreach (var unit in projectData.UnitsData)
                        {
                            NewBookingViewModel agentDetails = new NewBookingViewModel();
                            agentDetails.AgentId = agent.AgentId;
                            agentDetails.AgentName = agent.AgetName;
                            agentDetails.UnitSize  = unit.UnitSize;
                            agentDetails.CreatedDate = unit.CreatedDate;
                            agentDetails.UnitNumber = unit.UnitNumber;
                            agentDetails.Facing = unit.Facing;
                            agentDetails.ProjectId  = projectData.ProjectId;
                            agentDetails.ProjectName  = projectData.ProjectName;
                            agentDetails.Status = unit.Status;
                            lst.Add(agentDetails);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(agent.RejectedUnits))
                {
                    lstRejProjectDetails  = JsonSerializer.Deserialize<List<ProjectDetails>>(agent.RejectedUnits);
                    foreach (var projectData in lstRejProjectDetails)
                    {
                        foreach (var unit in projectData.UnitsData)
                        {
                            NewBookingViewModel agentDetails = new NewBookingViewModel();
                            agentDetails.AgentId = agent.AgentId;
                            agentDetails.AgentName = agent.AgetName;
                            agentDetails.UnitSize = unit.UnitSize;
                            agentDetails.CreatedDate = unit.CreatedDate;
                            agentDetails.UnitNumber = unit.UnitNumber;
                            agentDetails.Facing = unit.Facing;
                            agentDetails.ProjectId = projectData.ProjectId;
                            agentDetails.ProjectName = projectData.ProjectName;
                            agentDetails.Status = unit.Status;
                            lst.Add(agentDetails);
                        }
                    }
                }
                if (!string.IsNullOrEmpty(agent.BookingConfirmed))
                {
                    lstbookedProjectDetails = JsonSerializer.Deserialize<List<ProjectDetails>>(agent.BookingConfirmed);
                    foreach (var projectData in lstbookedProjectDetails)
                    {
                        foreach (var unit in projectData.UnitsData)
                        {
                            NewBookingViewModel agentDetails = new NewBookingViewModel();
                            agentDetails.AgentId = agent.AgentId;
                            agentDetails.AgentName = agent.AgetName;
                            agentDetails.UnitSize = unit.UnitSize;
                            agentDetails.CreatedDate = unit.CreatedDate;
                            agentDetails.UnitNumber = unit.UnitNumber;
                            agentDetails.Facing = unit.Facing;
                            agentDetails.ProjectId = projectData.ProjectId;
                            agentDetails.ProjectName = projectData.ProjectName;
                            agentDetails.Status = unit.Status;
                            lst.Add(agentDetails);
                        }
                    }
                }
            }
            return lst;
        }
        //Here Updating the Status of Unit either Confir or Rejected
        public bool UpdateStatusOfBooking(string aid,string pid,string un,string status,string type)
        {
            dynamic  agentDetails=null;
            if (type == "Admin")
            {
               agentDetails= _context.AdminDetails.Where(x => x.AdminUUID == aid).FirstOrDefault();
            }
            else if(type=="Agent") {
                agentDetails = _context.AgentRegistration.Where(x => x.AgentId == aid).FirstOrDefault();
            }
            else
            {
                return false;
            }
           
          var unitDetails = _context.ProjectUnitsData.Where(x => x.AgentId == aid && x.Projectuuid == pid && x.UnitNumber == un).FirstOrDefault();
            List<ProjectDetails> lstbookedData = new List<ProjectDetails>();
            List<ProjectDetails> lstrejectedData = new List<ProjectDetails>();
            List<ProjectDetails> lstNewrejectedData = new List<ProjectDetails>();
            if (agentDetails!=null)
            {
                if (!string.IsNullOrEmpty(agentDetails.BlockedUnits))
                {
                    List<ProjectDetails> lstDetails = JsonSerializer.Deserialize<List<ProjectDetails>>(agentDetails.BlockedUnits);
                    ProjectDetails BookedProject = new ProjectDetails();                   
                    List<BlockedUnits> lstBookedUnits = new List<BlockedUnits>();
                    List<ProjectDetails> lstNewBookedUnits = new List<ProjectDetails>();                   
                    ProjectDetails  project= lstDetails.Where(x=>x.ProjectId==pid).FirstOrDefault();
                    BlockedUnits  unit = project.UnitsData.Where(y=>y.UnitNumber==un).FirstOrDefault();
                    if (status =="Confirmed") {
                        BookedProject.ProjectId = project.ProjectId;
                        BookedProject.ProjectName = project.ProjectName;
                        unit.Status = StatusEnum.Booked.ToString();
                        unit.StatusConfiredDate = DateTime.Now.ToString();
                        unitDetails.status= StatusEnum.Booked.ToString();
                        lstBookedUnits.Add(unit);
                        BookedProject.UnitsData =lstBookedUnits ;                    
                        if (!string.IsNullOrEmpty(agentDetails.BookingConfirmed)) {
                            lstbookedData = JsonSerializer.Deserialize<List<ProjectDetails>>(agentDetails.BookingConfirmed);
                            lstNewBookedUnits = lstbookedData;
                            lstbookedData = lstbookedData.Where(x => x.ProjectId == pid).ToList();
                            if (lstbookedData != null && lstbookedData.Count>0) {
                                foreach (var data in lstbookedData)
                                {
                                    data.UnitsData.Add(unit);
                                }
                            }
                            else
                            {
                                lstNewBookedUnits.Add(BookedProject);
                            }
                            agentDetails.BookingConfirmed = JsonSerializer.Serialize(lstNewBookedUnits);
                        }
                        else
                        {
                            lstbookedData.Add(BookedProject);
                            agentDetails.BookingConfirmed = JsonSerializer.Serialize(lstbookedData);
                        }
                    }
                    else if(status =="Rejected") {
                        BookedProject.ProjectId = project.ProjectId;
                        BookedProject.ProjectName = project.ProjectName;
                        unit.Status = StatusEnum.Rejected.ToString();
                        unit.StatusConfiredDate = DateTime.Now.ToString();
                        unitDetails.status = StatusEnum.Available.ToString();
                        lstBookedUnits.Add(unit);
                        BookedProject.UnitsData = lstBookedUnits;
                        if (!string.IsNullOrEmpty(agentDetails.RejectedUnits)) {
                            lstrejectedData= JsonSerializer.Deserialize<List<ProjectDetails>>(agentDetails.RejectedUnits);
                            lstNewrejectedData = lstrejectedData;
                            lstrejectedData = lstrejectedData.Where(x => x.ProjectId == pid).ToList();
                            if (lstrejectedData != null && lstrejectedData.Count>0)
                            {
                                foreach (var data in lstrejectedData)
                                {
                                    data.UnitsData.Add(unit);
                                }
                            }
                            else
                            {
                                lstNewrejectedData.Add(BookedProject);
                            }
                            agentDetails.RejectedUnits = JsonSerializer.Serialize(lstNewrejectedData);
                        }
                        else {
                            lstrejectedData.Add(BookedProject);
                            agentDetails.RejectedUnits = JsonSerializer.Serialize(lstrejectedData);
                        }
                        
                    }
                    project.UnitsData.Remove(unit);                    
                    agentDetails.BlockedUnits = JsonSerializer.Serialize(lstDetails);
                    _context.SaveChanges();
                }
                return true;
            }
            return false;
        }
        //Forgot Password 
        public AdminDetails  CheckAdminExitsSendPassword(string id)
        {
             AdminDetails details  = _context.AdminDetails.Where(x => x.Email == id).FirstOrDefault();
            if (details!=null)
            {
                return details;
            }
            return null;
        }
        public bool UpdateActiveStatus(string agentId,bool status)
        {
          AgentRegistration agent=_context.AgentRegistration.Where(x => x.AgentId == agentId).FirstOrDefault();
            if (agent!=null)
            {
                if (status)
                {
                    agent.IsActive = false;
                }
                else { agent.IsActive = true; }
                _context.SaveChanges();
                return true;
            }
            return false;
        }
       public bool  DeleteAgentFromDb(string agentId)
        {
           AgentRegistration agent =_context.AgentRegistration.Where(x => x.AgentId == agentId).FirstOrDefault();
            if (agent!=null)
            {
                agent.Status = true;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
