using SarkPayOuts.Common;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using SarkPayOuts.Enums;

namespace SarkPayOuts.Repository
{
    public class AgentRepository:IAgentInterface
    {
        private readonly ApplicationDBContext _context; 
        public AgentRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public LoginViewModel LoginCheck(string userName, string password)
        {
            LoginViewModel model = new LoginViewModel();
            var loginDetails =_context.AgentRegistration.
                Where(x => x.Email == userName && x.Password == password && x.IsActive == true).FirstOrDefault();
            if (loginDetails!=null)
            {
                model.AgentName = loginDetails.AgetName;
                model.UserName = loginDetails.Email;
                model.Password = loginDetails.Password;
                model.IsActive = (bool)loginDetails.IsActive;
                model.AgentId = loginDetails.AgentId;
                model.Mobile = loginDetails.Mobile;
              
                return model;
            }
            return null;
        }
        public bool RegisterAgent(RegistrationModel model)
        {
            throw new NotImplementedException();
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
             var unitDetails =_context.AgentRegistration.Where(x => x.AgentId == model.AgentId).FirstOrDefault();
             var unitDetailsStatus = _context.ProjectUnitsData.Where(x => x.UnitNumber == model.UnitNumber && x.Projectuuid==model.ProjectId).FirstOrDefault();
            if (unitDetailsStatus!=null  || unitDetailsStatus.status== StatusEnum.Available.ToString())
            {
                unitDetailsStatus.status = StatusEnum.Blocked.ToString();
                unitDetailsStatus.BlockedDate = units.CreatedDate;
                unitDetailsStatus.ExpiredDate  = units.ExpiryDate ;
                unitDetailsStatus.AgentId = model.AgentId;
            }
             
            if (unitDetails!=null)
            {
                if (!string.IsNullOrEmpty (unitDetails.BlockedUnits))
                {
                  List<ProjectDetails> unitsData =JsonSerializer.Deserialize<List<ProjectDetails>>(unitDetails.BlockedUnits);
                    lstDetails = unitsData;
                   unitsData=unitsData.Where(x=>x.ProjectId ==model.ProjectId).ToList ();
                    if (unitsData != null && unitsData.Count>0)
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
                    lstDetails .Add(projectDetails );
                    unitDetails.BlockedUnits = JsonSerializer.Serialize(lstDetails);
                }
                _context.SaveChanges();
                return projectDetails;
            }
            return null;
        }
        public MyBookinsViewModel GetAgentsUnits(string agentId,string type)
        {
            MyBookinsViewModel units = new MyBookinsViewModel();
            dynamic list = null;
            if (type=="Admin") {
                list = _context.AdminDetails.Where(x => x.AdminUUID == agentId).FirstOrDefault(); } 
            else { 
                list = _context.AgentRegistration.Where(x => x.AgentId == agentId).FirstOrDefault(); }
            
            if (!string.IsNullOrEmpty(agentId))
            {
                if (list != null)
                {
                    if (!string.IsNullOrEmpty (list.BlockedUnits))
                    {
                        List<ProjectDetails> lstblockedUnits = JsonSerializer.Deserialize<List<ProjectDetails>>(list.BlockedUnits);
                        units.lstBlocked = lstblockedUnits;
                    }
                    if (!string.IsNullOrEmpty(list.RejectedUnits))
                    {
                        List<ProjectDetails> lstrejectedUnits = JsonSerializer.Deserialize<List<ProjectDetails>>(list.RejectedUnits);
                        units.lstRejected = lstrejectedUnits;
                    }
                    if (!string.IsNullOrEmpty(list.BookingConfirmed ))
                    {
                        List<ProjectDetails> lstBookedUnits = JsonSerializer.Deserialize<List<ProjectDetails>>(list.BookingConfirmed);
                        units.lstBooked = lstBookedUnits;
                    }
                }
            }
            return units;
        }
        public AgentRegistration CheckAdminExitsSendPassword(string id)
        {
            AgentRegistration details = _context.AgentRegistration.Where(x => x.Email == id).FirstOrDefault();
            if (details != null)
            {
                return details;
            }
            return null;
        }
        public DashboardViewModel DashboardData(string id)
        {
            DashboardViewModel model = new DashboardViewModel();
            var units = _context.ProjectUnitsData.ToList();
            model.TotalBookings = units.Where(x => x.status == "Booked" && x.AgentId==id).Count();
            model.TotalBlocked = units.Where(x => x.status == "Blocked" && x.AgentId == id).Count();
            model.NewBookings = units.Where(x => x.status == "Blocked" && x.BlockedDate.Contains(DateTime.Now.ToString("dd-MM-yyyy")) && x.AgentId == id).Count();
            return model;
        }
    }
}
