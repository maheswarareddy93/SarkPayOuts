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
    public class AdminLoginController : Controller
    {
        private readonly IAdminOperations _admin;
        private readonly ApplicationDBContext _context;
        const string SessionName = "_Name";
        const string SessionEmail = "_Age";
        const string SessionType = "_Type";

        public AdminLoginController(IAdminOperations admin, ApplicationDBContext context)
        {
            _context = context;
            _admin = admin;
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
    }
}
