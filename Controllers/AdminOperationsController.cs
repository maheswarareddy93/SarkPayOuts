using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SarkPayOuts.Interface;
using SarkPayOuts.Models;
using SarkPayOuts.Models.DbModels;

namespace SarkPayOuts.Controllers
{
    public class AdminOperationsController : Controller
    {
        [Obsolete]
        private IHostingEnvironment _hostingEnv;
        private IAdminOperations _operation;
        [Obsolete]
        public AdminOperationsController(IHostingEnvironment hostingEnv, IAdminOperations operation)
        {
            _hostingEnv = hostingEnv;
            _operation = operation;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ReadExcelData()
        {
            return View();
        }
        [HttpPost]
        [Obsolete]
        public IActionResult ImportExcelFile(IFormCollection collection)
        {
            string result = string.Empty;
            string filename = string.Empty;
            ProjectsData projectDetails = new ProjectsData();
            List<ProjectUnitsData> lstUnitsData = new List<ProjectUnitsData>();
            try
            {
                long size = 0;
                var file = Request.Form.Files;
                string unitsize = string.Empty;
                string area = string.Empty;
                string facing = string.Empty;
                string mortigaze = string.Empty;
                string unitNuber = string.Empty;
                string Id = string.Empty;
                if (file.Count > 0)
                {
                    filename = ContentDispositionHeaderValue.Parse(file[0].ContentDisposition).FileName.Trim('"');
                    if (Path.GetExtension(filename) == ".xlsx" || Path.GetExtension(filename) == ".csv")
                    {
                        projectDetails.ProjectName = filename.Split(".")[0].ToUpper();

                        projectDetails.status = "Available";
                        Id = _operation.AddProjectToDB(projectDetails);
                        string FilePath = _hostingEnv.WebRootPath + "\\ApplicationDocs\\UnitsData\\";
                        string destinationPath = Path.Combine(FilePath, filename);
                        size += file[0].Length;
                        var fileLocation = new FileInfo(destinationPath);
                        using (var fileStream = new FileStream(destinationPath, FileMode.Create))
                        {
                            file[0].CopyTo(fileStream);
                            if (fileStream.Length > 0)
                            {

                            }
                            else
                            {
                                return Json("Upload File Having No Data!,Please Check.");
                            }
                        }
                        result = destinationPath;
                        List<ProjectUnitsData> users = new List<ProjectUnitsData>();
                        // var fileName = "./Users.xlsx";
                        // For .net core, the next line requires the NuGet package, 
                        // System.Text.Encoding.CodePages
                        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                        using (var stream = System.IO.File.Open(destinationPath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = ExcelReaderFactory.CreateReader(stream))
                            {
                                int i = 1;
                                while (reader.Read()) //Each row of the file
                                {
                                    if (!string.IsNullOrEmpty(reader.GetValue(0).ToString()))
                                    { unitNuber = reader.GetValue(0).ToString(); }
                                    else
                                    {
                                        unitNuber = "";
                                    }
                                    if (!string.IsNullOrEmpty(reader.GetValue(1).ToString())) { area = reader.GetValue(1).ToString(); }
                                    else
                                    { area = "243"; }
                                    if (!string.IsNullOrEmpty(reader.GetValue(2).ToString())) { facing = reader.GetValue(2).ToString(); }
                                    else
                                    { facing = "East"; }
                                    if (!string.IsNullOrEmpty(reader.GetValue(3).ToString())) { mortigaze = reader.GetValue(3).ToString(); }
                                    else { mortigaze = "sa"; }
                                    users.Add(new ProjectUnitsData
                                    {
                                        UnitNumber = unitNuber,
                                        UnitSize = area,
                                        Facing = facing,
                                        Projectuuid = Id,
                                    });
                                    i = i + 1;
                                }
                                if (users.Count > 0)
                                {
                                    lstUnitsData.AddRange(users);
                                    _operation.PopulateUnitsDataToDb(lstUnitsData);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //_emailService.SendEmail("maheswarareddyk93@gmail.com", "", "Excel Upload", ex.ToString(), null);
            }
            return Json("Data Uploaded Successfully");
        }
        public IActionResult MyBookings()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return View();
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        public IActionResult Agents()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                List<AgentModel> agents = _operation.GetAgentsData();
                AgentViewModel viewModel = new AgentViewModel();
                viewModel.AgentsList = agents;
                return View(viewModel);
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        public IActionResult NewBookings()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                List<NewBookingViewModel> lstDetails =_operation.GetNewBookings();
                return View(lstDetails);
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        public IActionResult DashBoard()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                return View();
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        [HttpPost]
        public JsonResult  RegisterAgent(RegistrationModel model)
        {
           string response = string.Empty;
           if(! _operation.CheckAgentExists(model.Mobile, model.Email))
            {
                if(_operation.RegisterNewAgent(model))
                    response = "Agent Registered Successfully";
            }
            else { response = "Agent Already Exits with Email/Mobile"; }
            return Json(response);
        }
        [HttpPost]
        public JsonResult UpdatingBlockingUnitsStatus()
        {
            _operation.UpdatingBlockingUnitsStatus();
            return Json("");
        }
        public IActionResult AdminsDataIntoDb(string id)
        {

            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                string  adminId = HttpContext.Session.GetString("AdminId");
                UnitModel model = new UnitModel();
                string[] arr = id.Split(",");
                model.UnitNumber = arr[0].Split(":  ")[1];
                model.UnitSize = arr[1].Split(":  ")[1];
                model.Facing = arr[2].Split(":  ")[1];
                model.AgentId=adminId;
                model.ProjectName = arr[3].Split("-")[0].ToUpper();
                model.ProjectId = arr[4];
                ProjectDetails units = _operation .AddBlockedUnitsToDb(model);
                return Json("", units);
            }
            return RedirectToAction("Index","AdminLogin");
        }
    }
}
