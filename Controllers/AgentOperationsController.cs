using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SarkPayOuts.Helper;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
using System.Collections.Generic;
using System.Linq;

namespace SarkPayOuts.Controllers
{
    public class AgentOperationsController : Controller
    {
        private readonly IAgentInterface _agentOperations;
        private readonly IAdminOperations _operations;
        public AgentOperationsController(ApplicationDBContext context, IAgentInterface agentOperations,IAdminOperations operations )
        {
            _agentOperations = agentOperations;
            _operations = operations;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AgentsDataIntoDb(string id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_AgentId"))) {
                string agentId = HttpContext.Session.GetString("_AgentId");
            UnitModel model = new UnitModel();
            string[] arr = id.Split(",");
            model.UnitNumber = arr[0].Split(":  ")[1];
            model.UnitSize = arr[1].Split(":  ")[1];
            model.Facing = arr[2].Split(":  ")[1];
            model.AgentId = agentId;
            model.ProjectName = arr[3].Split("-")[0].ToUpper();
            model.ProjectId = arr[4];
            ProjectDetails units = new ProjectDetails();
            units=  _agentOperations.AddBlockedUnitsToDb(model);
             new JsonResult(new { data = units });
            }
            return RedirectToAction("Index", "AgentLogin");
        }
        public IActionResult MyBookings()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_AgentId")))
            {
                string id = HttpContext.Session.GetString("_AgentId");
                string type= HttpContext.Session.GetString("_Type");
                MyBookinsViewModel units = _agentOperations.GetAgentsUnits(id,type);
                return View(units);
            }
            return RedirectToAction("Index","AgentLogin");
        }

        public IActionResult Dashboard()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_AgentId")))
            {
                string id = HttpContext.Session.GetString("_AgentId");
                DashboardViewModel model = _agentOperations.DashboardData(id);
                return View(model);
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        public IActionResult UpdateBookingStatus(string aid, string pid, string un, string state, string type,string name)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_AgentId")))
            {
                if (!string.IsNullOrEmpty(aid) && !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(un))
                {
                    if (_operations.UpdateStatusOfBooking(aid, pid, un, state, type,name,""))
                    {
                        if (state == "Confirmed")
                        {
                            return Json("Unit " + un + " Booked  Successfully");
                        }
                        else if (state == "Rejected")
                        {
                            return Json("Unit " + un + " Rejected Please Contact Admin...");
                        }
                        else { return Json(""); }
                    }
                }
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        public IActionResult Profile()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_AgentId")))
            {
                List<AgentModel> lst = _operations.GetAgentsData();
               AgentModel model =lst.Where(x => x.AgentId == HttpContext.Session.GetString("_AgentId").ToString()).FirstOrDefault();
                return View(model);
            }
            return RedirectToAction("Index", "AgentLogin");
        }
        public JsonResult GettingBlockedUnitsData()
        {
            string id = HttpContext.Session.GetString("_AgentId");
            MyBookinsViewModel units = _agentOperations.GetAgentsUnits(id,"Agent");
            List<ProjectDetails> lstUnits = units.lstBlocked.ToList();
           
            
           
            return Json(lstUnits);
        }
    }
}
