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
            if (agentDetails!=null) {
                return true ;
            }
            return false  ;
        }
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

        public bool UpdatingBlockingUnitsStatus()
        {
            try
            {
              var agents=  _context.AgentRegistration.ToList();
                if (agents!=null && agents.Count>0)
                {
                    foreach (var agent in agents)
                    {
                        if (agent.BlockedUnits!=null)
                        {
                            List<ProjectDetails> lstNewList = new List<ProjectDetails>();
                            List<BlockedUnits> lstNewBlockedUnits = new List<BlockedUnits>();
                            
                            AgentRegistration agentUpdate = _context.AgentRegistration.Where(x => x.AgentId == agent.AgentId).FirstOrDefault();
                            List<ProjectDetails> lstDetails = JsonSerializer.Deserialize<List<ProjectDetails>>(agent.BlockedUnits);
                            List<ProjectDetails> lstRejectedUnits = new List<ProjectDetails>();
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
                                            BlockedUnits rejected = new BlockedUnits();
                                            rejected.Id = unitdata.Id;
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
                                            _context.SaveChanges();
                                            if (!string.IsNullOrEmpty(agentUpdate.RejectedUnits))
                                            {
                                                lstRejectedUnits = JsonSerializer.Deserialize<List<ProjectDetails>>(agentUpdate.RejectedUnits);                                               
                                                rejectedUnits.ProjectId = project.ProjectId;
                                                rejectedUnits.ProjectName = project.ProjectName;
                                                rejectedUnits.UnitsData = lstRejected;
                                                lstRejectedUnits.Add(rejectedUnits);
                                            }
                                            else
                                            {
                                                rejectedUnits.UnitsData = lstRejected;
                                                lstRejectedUnits.Add(rejectedUnits);
                                            }
                                            
                                            //UpdateProjectUnitStatus(agent, project, project.ProjectId, unitdata.UnitNumber);
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
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
            units.Id = CommonMethods.GenerateuniqueId();
            units.UnitNumber = model.UnitNumber;
            units.UnitSize = model.UnitSize;
            units.Facing = model.Facing;
            units.CreatedDate = DateTime.Now.ToString("MM-dd-yyyy hh:mm tt");
            units.ExpiryDate = DateTime.Now.AddDays(2).ToString("MM-dd-yyyy hh:mm tt");
            units.AgentId = model.AgentId;
            units.AdminId = model.AdminId ;
            units.Status = StatusEnum.Blocked.ToString();
            units.ProjectName = model.ProjectName;
            projectDetails.ProjectName = model.ProjectName;
            lstunits.Add(units);
            projectDetails.UnitsData = lstunits;
            projectDetails.ProjectId = model.ProjectId;
            var unitDetails = _context.AdminDetails .Where(x => x.AdminUUID  == model.AgentId ).FirstOrDefault();
            var unitDetailsStatus = _context.ProjectUnitsData.Where(x => x.UnitNumber == model.UnitNumber && x.Projectuuid == model.ProjectId).FirstOrDefault();
            if (unitDetailsStatus != null && unitDetailsStatus.status == StatusEnum.Available.ToString())
            {
                unitDetailsStatus.status = StatusEnum.Blocked.ToString();
                unitDetailsStatus.BlockedDate = units.CreatedDate;
                unitDetailsStatus.ExpiredDate = units.ExpiryDate;
                unitDetailsStatus.AgentId =model.AgentId ;
            }

            if (unitDetails != null)
            {
                if (!string.IsNullOrEmpty(unitDetails.BlockedUnits))
                {
                    List<ProjectDetails> unitsData = JsonSerializer.Deserialize<List<ProjectDetails>>(unitDetails.BlockedUnits);
                    unitsData = unitsData.Where(x => x.ProjectId == model.ProjectId).ToList();
                    if (unitsData != null)
                    {
                        foreach (var data in unitsData)
                        {
                            data.UnitsData.Add(units);
                        }
                    }
                    else
                    {
                        unitsData.Add(projectDetails);
                    }
                    unitDetails.BlockedUnits = JsonSerializer.Serialize(unitsData);
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

        public List<NewBookingViewModel> GetNewBookings()
        {
            List<NewBookingViewModel> lst = new List<NewBookingViewModel>();
            
            var agents =_context.AgentRegistration.ToList();
            foreach (var agent in agents)
            {
                List<ProjectDetails> lstProjectDetails = new List<ProjectDetails>();
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
            }
            return lst;
        }
    }
}
