using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;

namespace SarkPayOuts.Controllers
{
    
    public class ProjectLayoutController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _hostingEnv;
        private IConfiguration _Configuration;
        private IProjectsData _projectOperations;
        private IAdminOperations _operations;
        [Obsolete]
        public ProjectLayoutController(IHostingEnvironment hostingEnv, IConfiguration Configuration, IProjectsData projectOperations, IAdminOperations operations)
        {
            _hostingEnv = hostingEnv;
            _Configuration = Configuration;
            _operations = operations;
            _projectOperations = projectOperations;
        }
        public IActionResult Index_1(string id)
        {            
            LayOutViewModel view = _projectOperations.GetProjectUnitsStatusCount(id);
            view.Type = GetSessionId();
            view.ProjectId = id;
            view.lstAgents = _operations.GetAgentsData();
            view.Name = GetProjectsName(id);
            return View(view);

        }
        public IActionResult Index_2(string id)
        {
           // HttpContext.Session.SetString("_ProjectId", id);
            LayOutViewModel view = _projectOperations.GetProjectUnitsStatusCount(id);
            view.Type = GetSessionId();
            view.ProjectId = id;
            view.lstAgents = _operations.GetAgentsData();
            view.Name = GetProjectsName(id);
            return View(view);
        }
        public IActionResult Index_3(string id)
        {
            HttpContext.Session.SetString("_ProjectId", id);
            LayOutViewModel view = _projectOperations.GetProjectUnitsStatusCount(id);
            view.Type = GetSessionId();
            view.ProjectId = id;
            view.lstAgents = _operations.GetAgentsData();
            return View(view);
        }
        public string GetSessionId()
        {
            string agentType = string.Empty;
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_Type")))
            {
                agentType = HttpContext.Session.GetString("_Type");
            }
            return agentType;
        }
        [HttpPost]
        public JsonResult GetUnitDetails(string id)
        {
            UnitModel model = new UnitModel();
            if (!string.IsNullOrEmpty(id)) {
                string[] strArr = id.Split("-");
                string projectName = strArr[0];
                string unitId = strArr[1];
                string ProjectId= _projectOperations.GetProjectIdByName(projectName);                
                model= _projectOperations.GetUnitdetailsByProjectId(ProjectId,unitId);
                return Json(model);
            }
            return Json("");
        }
        public JsonResult GetUnitDataFromDb(string id) 
        {
            List<BlockedUnits> lstUnits= _projectOperations.GetUnitsDataForView(id);
            return Json(lstUnits);
        }
        public string GetProjectsName(string id) {
            return  (from c in  _projectOperations.GetProjectsData().Where(x => x.projectuuid == id) select c.ProjectName).FirstOrDefault().ToString();
        }
    }
}
