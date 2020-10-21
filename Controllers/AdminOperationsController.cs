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
        private readonly IAgentInterface _agentOperations;
        [Obsolete]
        public AdminOperationsController(IHostingEnvironment hostingEnv, IAdminOperations operation, IAgentInterface agentOperations)
        {
            _hostingEnv = hostingEnv;
            _operation = operation;
            _agentOperations = agentOperations;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddLayOutData()
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
                                        Mortigaze = mortigaze,
                                        status = "Available"
                                    }) ;
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
            return RedirectToAction("","");
        }
        public IActionResult MyBookings()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                string id = HttpContext.Session.GetString("AdminId");
                string type = HttpContext.Session.GetString("_Type");
                MyBookinsViewModel units = _agentOperations.GetAgentsUnits(id, type);
                return View(units);
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
                List<NewBookingViewModel> lstDetails = _operation.GetNewBookings(HttpContext.Session.GetString("AdminId"));
                return View(lstDetails);
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        public IActionResult DashBoard()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                DashboardViewModel model=_operation.DashboardData();
                return View(model);
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        public IActionResult RegisterAgent()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                RegistrationModel model = new RegistrationModel();
                return View(model);
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        //Register New Agent
        [HttpPost]
        public IActionResult RegisterAgent(RegistrationModel model)
        {
            if (!_operation.CheckAgentExists(model.Mobile, model.Email))
            {
                if (ModelState.IsValid)
                {
                    if (_operation.RegisterNewAgent(model))
                    {
                        return RedirectToAction("Agents", "AdminOperations");
                    }
                }
            }
            return View();
        }
        //Auto Update Function To update data After 48 hours
        [HttpPost]
        public JsonResult UpdatingBlockingUnitsStatus()
        {
            _operation.UpdatingBlockingUnitsStatus();
            return Json("");
        }
        //Admin Blocked UnitData Insertion Operation
        public IActionResult AdminsDataIntoDb(string id)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                string adminId = HttpContext.Session.GetString("AdminId");
                UnitModel model = new UnitModel();
                string[] arr = id.Split(",");
                model.UnitNumber = arr[0].Split(":  ")[1];
                model.UnitSize = arr[1].Split(":  ")[1];
                model.Facing = arr[2].Split(":  ")[1];
                model.AgentId = adminId;
                model.ProjectName = arr[3].Split("-")[0].ToUpper();
                model.ProjectId = arr[4];
                ProjectDetails units = _operation.AddBlockedUnitsToDb(model);
                return Json("", units);
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        //Updating Booking Status either "Booked" or "Rejected" based on Admin Action
        public IActionResult UpdateBookingStatus(string aid, string pid, string un, string state, string type)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminId")))
            {
                if (!string.IsNullOrEmpty(aid) && !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(un))
                {
                    if (_operation.UpdateStatusOfBooking(aid, pid, un, state, type))
                    {
                        if (state  =="Confirmed")
                        {
                            return Json("Unit " + un + " Booked  Successfully");
                        }
                        else if (state  == "Rejected")
                        {
                            return Json("Unit " + un + " Rejected Please Contact Admin...");
                        }
                        else { return Json(""); }
                    }
                }
            }
            return RedirectToAction("Index", "AdminLogin");
        }
        //Updating the Active Status of Agent
        public JsonResult UpdateStatus(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string[] strArray = id.Split("_");
                if (_operation.UpdateActiveStatus(strArray[0].Trim(), bool.Parse(strArray[1])))
                {
                    return Json("Agent Status  Updated Successfully.");
                }
            }
            return Json("");
        }
        //Removing the Agent Data from Db
        public JsonResult UpdatingDeletedStatus(string id)
        {
            if (_operation.DeleteAgentFromDb(id))
            {
                return Json("Agent Removed Successfully");
            }
            return Json("something went wrong Please Contact Admin");
        }

        public IActionResult ViewLayout()
        {
          return View();
        }
        public IActionResult GetUnits(string id)
        {
            List<ViewLayoutModel> model = _operation.GetAllUnitsData(id);
            return PartialView("_ProjectsUnitsData",model);
        }
    }
}
