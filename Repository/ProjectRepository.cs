using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
using System.Text.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System;
using SarkPayOuts.Enums;

namespace SarkPayOuts.Repository
{
    public class ProjectRepository : IProjectsData
    {
        private readonly ApplicationDBContext _context;
        public ProjectRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public string GetProjectIdByName(string name)
        {
            var details = _context.ProjectsData.Where(x => x.ProjectName == name.ToUpper()).FirstOrDefault();
            return details.projectuuid;
        }

        public UnitModel GetUnitdetailsByProjectId(string projectId, string unitno)
        {
            if (!string.IsNullOrEmpty(projectId))
            {
                var model = (from units in _context.ProjectUnitsData.Where(x => x.Projectuuid == projectId && x.UnitNumber == unitno)
                             select new UnitModel
                             {
                                 UnitNumber = units.UnitNumber,
                                 UnitSize = units.UnitSize,
                                 Facing = units.Facing,
                                 ProjectId = units.Projectuuid,
                                 Status = units.status
                             }).FirstOrDefault();
                return model;
            }
            return null;
        }
        public List<ProjectsData> GetProjectsData()
        {
            List<ProjectsData> lstProjects = _context.ProjectsData.ToList();
            return lstProjects;
        }
        public bool CheckBlockedUnits()
        {
            List<AgentRegistration> data = _context.AgentRegistration.ToList();
            if (data != null && data.Count > 0)
            {
                foreach (var agent in data)
                {
                    string agentId = agent.AgentId;
                    string agentsBlockedUnits = agent.BlockedUnits;
                    if (!string.IsNullOrEmpty(agentsBlockedUnits))
                    {
                        List<BlockedUnits> units = JsonSerializer.Deserialize<List<BlockedUnits>>(agentsBlockedUnits);
                        List<BlockedUnits> unitsRejected = JsonSerializer.Deserialize<List<BlockedUnits>>(agentsBlockedUnits);
                        if (units != null && units.Count > 0)
                        {
                            foreach (var dates in units)
                            {
                                DateTime createdDate = Convert.ToDateTime (dates.CreatedDate);
                                DateTime expiredDate = Convert.ToDateTime(dates.ExpiryDate) ;
                                if (expiredDate< DateTime.Now)
                                {
                                    unitsRejected.Add(dates);
                                    units.Remove(dates);
                                }
                            }
                            agent.BlockedUnits = JsonSerializer.Serialize(units);
                            agent.RejectedUnits  = JsonSerializer.Serialize(unitsRejected);
                            data.Where(x => x.AgentId == agent.AgentId).FirstOrDefault();
                            data.Add(agent);
                        }
                    }
                }
                _context.SaveChanges();
            }
            return false;
        }
        public List<BlockedUnits> GetUnitsDataForView(string id)
        {
            List<BlockedUnits> lstUnits = new List<BlockedUnits>();
            var units = _context.ProjectUnitsData.Where(x=>x.Projectuuid==id).ToList();
            foreach (var data in units)
            {
                BlockedUnits unit = new BlockedUnits();
                unit.UnitNumber = data.UnitNumber;
                unit.Status  = data.status ;
                unit.AgentId  = data.AgentId ;
                unit.ProjectId  = data.Projectuuid ;
                var ProjectName = _context.ProjectsData.Where(x => x.projectuuid == data.Projectuuid).FirstOrDefault();
                unit.ProjectName = ProjectName.ProjectName;
                lstUnits.Add(unit);
            }
            return lstUnits;
        }
        public  LayOutViewModel GetProjectUnitsStatusCount(string projectId)
        {
            LayOutViewModel objData = new LayOutViewModel();
            List<ProjectUnitsData> lstUnits =_context.ProjectUnitsData.Where(x=>x.Projectuuid==projectId).ToList();
            if (lstUnits!=null && lstUnits.Count >0) {
                objData.BolckedUnits = lstUnits.Count();
                objData.AvailableUnits = lstUnits.Where(x => x.status == StatusEnum.Available.ToString() || x.status == null).Count();
                objData.BolckedUnits = lstUnits.Where(x => x.status == StatusEnum.Blocked.ToString()).Count();
                objData.BookedUnits = lstUnits.Where(x => x.status == StatusEnum.Booked.ToString()).Count();
                return objData;
            }
            return null;
        }
    }


}
