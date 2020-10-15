using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SarkPayOuts.Helper;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;

namespace SarkPayOuts.Controllers
{
    public class AgentLogin : Controller
    {
        private readonly IAgentInterface _agent;
        private readonly ApplicationDBContext _context;
        const string SessionName = "_Name";
        const string SessionEmail = "_Age";
        const string SessionAgentId = "_AgentId";
        const string SessionType = "_Type";
        public AgentLogin(IAgentInterface agent, ApplicationDBContext context)
        {
            _context = context;
            _agent = agent;
        }
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionAgentId)))
            {
                return RedirectToAction("MyBookings", "AgentOperations");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionAgentId)))
            {
                return RedirectToAction("MyBookings", "AgentOperations");
            }
            if (ModelState.IsValid)
            {
                LoginViewModel agent = _agent.LoginCheck(model.UserName, model.Password);
                if (agent != null)
                {
                    HttpContext.Session.SetString(SessionName, agent.AgentName);
                    HttpContext.Session.SetString(SessionEmail,agent.UserName);
                    HttpContext.Session.SetString(SessionAgentId, agent.AgentId);
                    HttpContext.Session.SetString(SessionType, "Agent");
                    return RedirectToAction("MyBookings", "AgentOperations");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password is wrong");
                }
            }
            return View();
        }
        public IActionResult RegisterAgent()
        {
            RegistrationModel model = new RegistrationModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult RegisterAgent(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                _agent.RegisterAgent(model);
            }
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.SetString(SessionAgentId,"");
            return RedirectToAction("Index","AgentLogin");
        }
    }
}
