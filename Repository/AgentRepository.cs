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
            if (unitDetailsStatus!=null  && unitDetailsStatus.status== StatusEnum.Available.ToString())
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
                  unitsData=unitsData.Where(x=>x.ProjectId ==model.ProjectId).ToList ();
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
                    lstDetails .Add(projectDetails );
                    unitDetails.BlockedUnits = JsonSerializer.Serialize(lstDetails);
                }
                _context.SaveChanges();
                return projectDetails;
            }
            return null;
        }
        public List<ProjectDetails> GetAgentsUnits(string agentId)
        {
            List<ProjectDetails> blockedUnits = new List<ProjectDetails>();
            if (!string.IsNullOrEmpty(agentId))
            {
              var list = _context.AgentRegistration.Where(x => x.AgentId == agentId).FirstOrDefault();
                if (list != null)
                {
                    if (!string.IsNullOrEmpty (list.BlockedUnits))
                    {
                         blockedUnits = JsonSerializer.Deserialize<List<ProjectDetails>>(list.BlockedUnits);
                    }
                    if (!string.IsNullOrEmpty(list.BookingConfirmed))
                    {
                         blockedUnits = JsonSerializer.Deserialize<List<ProjectDetails>>(list.BlockedUnits);
                    }
                    if (!string.IsNullOrEmpty(list.RejectedUnits))
                    {
                       blockedUnits = JsonSerializer.Deserialize<List<ProjectDetails>>(list.BlockedUnits);
                    }
                }
            }
            return blockedUnits;
        }
    }
}
