using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SarkPayOuts.Interface
{
   public  interface IProjectsData
    {
        string GetProjectIdByName(string name);
        UnitModel GetUnitdetailsByProjectId(string projectId, string unitno);
        List<ProjectsData> GetProjectsData();
        bool CheckBlockedUnits();
        List<BlockedUnits> GetUnitsDataForView(string id);
    }
}
