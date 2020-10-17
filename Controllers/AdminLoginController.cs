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
    public class AdminLoginController : Controller
    {
        private readonly IAdminOperations _admin;
        private readonly ApplicationDBContext _context;
        private readonly IEmailSender _sender;
        const string SessionName = "_Name";
        const string SessionEmail = "_Age";
        const string SessionType = "_Type";
        MessageCreater cs = new MessageCreater();
        public AdminLoginController(IAdminOperations admin, ApplicationDBContext context,IEmailSender sender)
        {
            _context = context;
            _admin = admin;
            _sender = sender;
        }
        public IActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();

                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId"))) 
            {
                return RedirectToAction("MyBookings", "AdminOperations");
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString ("AdminId")))
            {
                return RedirectToAction("MyBookings", "AdminOperations");
            }
            if (ModelState.IsValid)
            {
                LoginViewModel agent = _admin.AdminLoginCheck(model.UserName, model.Password);
                if (agent != null)
                {                 
                    HttpContext.Session.SetString(SessionName, agent.AgentName);
                    HttpContext.Session.SetString(SessionEmail,agent.UserName);
                    HttpContext.Session.SetString ("AdminId", agent.AdminId);
                    HttpContext.Session.SetString(SessionType,"Admin");
                    return RedirectToAction("MyBookings", "AdminOperations");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password is wrong");
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.SetString("AdminId", "");
            return RedirectToAction("Index","AdminLogin");
        }
        public JsonResult ForgotPassword(string id) {
            if (!string.IsNullOrEmpty(id))
            {
              AdminDetails data = _admin.CheckAdminExitsSendPassword(id);
                if (data!=null)
                {
                  string message =cs.ForgotPasswordSMS(data,"https://Sarkpayouts.in/AdminLogin");
                    _sender.SendEmail(data.Email,"","ForgotPassword",message,null);
                    _sender.SendSMS(data.Name,message,data.Mobile);
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
