using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SarkPayOuts.Helper;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;
using System.Collections.Generic;

namespace SarkPayOuts.Controllers
{
    public class AgentOperationsController : Controller
    {
        private readonly IAgentInterface _agentOperations;
        
        public AgentOperationsController(ApplicationDBContext context, IAgentInterface agentOperations)
        {
            _agentOperations = agentOperations;
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
            
            ProjectDetails  units=  _agentOperations.AddBlockedUnitsToDb(model);
            return Json("", units);
            }
            return RedirectToAction("Index", "AgentLogin");
        }

        public IActionResult MyBookings()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("_AgentId")))
            {
                string id = HttpContext.Session.GetString("_AgentId");
                List<ProjectDetails> units = _agentOperations.GetAgentsUnits(id);
                return View(units);
            }
            return RedirectToAction("Index","AgentLogin");
        }
        
    }
}
