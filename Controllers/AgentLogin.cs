using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SarkPayOuts.Helper;
using SarkPayOuts.Interface;
using SarkPayOuts.MailsAndMessages;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;

namespace SarkPayOuts.Controllers
{
    public class AgentLogin : Controller
    {
        private readonly IAgentInterface _agent;
        private readonly ApplicationDBContext _context;
        private readonly IEmailSender _sender;
        MessageCreater cs = new MessageCreater();
        const string SessionName = "_Name";
        const string SessionEmail = "_Age";
        const string SessionAgentId = "_AgentId";
        const string SessionType = "_Type";
        public AgentLogin(IAgentInterface agent, ApplicationDBContext context,IEmailSender sender)
        {
            _context = context;
            _agent = agent;
            _sender = sender;
        }
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SessionAgentId)))
            {
                return RedirectToAction("Dashboard", "AgentOperations");
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
        public JsonResult ForgotPassword(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                AgentRegistration data = _agent.CheckAdminExitsSendPassword(id);
                if (data != null)
                {
                    //string message = cs.ForgotPasswordSMS(data, "https://Sarkpayouts.in/AdminLogin");
                    //_sender.SendEmail(data.Email, "", "ForgotPassword", message, null);
                    //_sender.SendSMS(data.Name, message, data.Mobile);
                }
                else
                {
                    return Json("Please Enter Valid Email");
                }
            }
            return Json("Please Enter Email Id");
        }
    }
}
