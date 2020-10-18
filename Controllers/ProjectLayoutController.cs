using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SarkPayOuts.Helper;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;

namespace SarkPayOuts.Controllers
{
    
    public class ProjectLayoutController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _hostingEnv;
        private IConfiguration _Configuration;
        private IProjectsData _projectOperations;
        [Obsolete]
        public ProjectLayoutController(IHostingEnvironment hostingEnv, IConfiguration Configuration, IProjectsData projectOperations)
        {
            _hostingEnv = hostingEnv;
            _Configuration = Configuration;
            _projectOperations = projectOperations;
        }
        public IActionResult Index_1(string id)
        {
            HttpContext.Session.SetString("_ProjectId", id);
            LayOutViewModel view = _projectOperations.GetProjectUnitsStatusCount(id);
            view.Type = GetSessionId();
            view.ProjectId = id;
            return View(view);

        }
        public IActionResult Index_2(string id)
        {
            HttpContext.Session.SetString("_ProjectId", id);
            LayOutViewModel view = _projectOperations.GetProjectUnitsStatusCount(id);
            view.Type = GetSessionId();
            view.ProjectId = id;
            return View();
        }
        public IActionResult Index_3(string id)
        {
            HttpContext.Session.SetString("_ProjectId", id);
            LayOutViewModel view = _projectOperations.GetProjectUnitsStatusCount(id);
            view.Type = GetSessionId();
            view.ProjectId = id;
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
    }
}
