using Hospital_Management_System.DAL;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq.Dynamic;
using System.Data;
using Microsoft.Ajax.Utilities;
using HospitalManagementLibrary.Enum;
using ArrayToPdf;

namespace Hospital_Management_System.Controllers
{
    public class UsersController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection formCollection)
        {
            DBHelper helper = new DBHelper();
            int IsUser = helper.GetRegister(formCollection["Username"], formCollection["Password"]);
            int IsAdmin = helper.IsAdminCheck(formCollection["Username"], formCollection["Password"]);
            if (IsUser == 1)
            {
                FormsAuthentication.SetAuthCookie(formCollection["Username"], true);
                return RedirectToAction("Index", "Patient");
            }
            else if(IsAdmin == 1)
            {
                FormsAuthentication.SetAuthCookie(formCollection["Username"], true);               
                return RedirectToAction("Admin", "Users");
            }
            else
            {
                TempData["LoginFail"] = "Username or password is wrong...";
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Users");
        }

        [Authorize]
        public ActionResult Admin()
        {
            return View();
        }

        public ActionResult CreateUsers()
        {
            Registration ObjRegistration = new Registration();
            return PartialView("_Registration", ObjRegistration);

        }

        [HttpPost]
        public ActionResult CreateUsers(Registration registration)
        {

            bool authenticated = Request.IsAuthenticated;
            var AdminUser = User.Identity.Name;
            DBHelper helper = new DBHelper(); 
            registration.Created_by = AdminUser;
            registration.Created_date = DateTime.Now.ToString();
            registration.Modified_by = AdminUser;
            registration.Modified_date = DateTime.Now.ToString();
            var isRegExist = helper.GetByUsername(registration.UserName);
            if (isRegExist.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    var success = helper.AddUser(registration);
                    if (success)
                    {
                        ViewBag.Message = "Registration Successfull";
                        ModelState.Clear();
                    }
                }
                else
                {
                    return PartialView("_Registration");
                }
            }
            else
            {
                ViewBag.Exist = "exist!";
                return View("Admin");
            }
            return View("Admin");
        }


        
        public ActionResult UserInfo()
        {
            return View();
        }

        public ActionResult PatientInfo()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult UsersData()
        {
            try
            {
                DBHelper helper = new DBHelper();
                var start = Convert.ToInt32(Request.Form["start"]);
                var length = Convert.ToInt32(Request.Form["length"]);
                var searchValue = Convert.ToString(Request.Form["search[value]"]);
                //var searchValue = Request.Form["search[value]"];
                string columnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string direction = Request["order[0][dir]"];
                List<Registration> registeredData = helper.GetReg(start, length, searchValue);
                registeredData = registeredData.OrderBy(columnName + " " + direction).ToList();
                int count = helper.GetFullCountOfUsers(searchValue);
                return Json(new { data = registeredData, recordsTotal = count, recordsFiltered = count }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                string excep = ex.Message;
                return View();
            }
        }

        // GET: Users/Get?operation&id
        public ActionResult Get(string operation, int id)
        {
            if (id > 0)
            {
                DBHelper helper = new DBHelper();
               Registration objRegistration = helper.GetByIdOfUser(id);
                if (operation == Operations.View.ToString())
                {
                    objRegistration.ActionType = Operations.View;
                    return PartialView("_Registration", objRegistration);
                }
                else if (operation == Operations.Edit.ToString())
                {
                    objRegistration.ActionType = Operations.Edit;
                    return PartialView("_Registration", objRegistration);
                }
                else
                {
                    return PartialView("_DeleteUser", objRegistration);
                }
            }
            else
            {
                Patient objPatient = new Patient
                {
                    ActionType = Operations.Add
                };
                return PartialView("_Registration", objPatient);
            }
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            try
            {
                DBHelper helper = new DBHelper();
                result = helper.DeleteUser(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateUser(Registration objRegistration)
         {
            bool result = false;
            DBHelper helper = new DBHelper();
            try
            {
                    var dataresult = helper.GetByIdOfUser(objRegistration.Id);
                    bool authenticated = Request.IsAuthenticated;
                    var AdminUser = User.Identity.Name;
                    objRegistration.Password = dataresult.Password;
                    objRegistration.IsAdmin = dataresult.IsAdmin;
                    objRegistration.Created_by = dataresult.Created_by;
                    objRegistration.Created_date = dataresult.Created_date;
                    objRegistration.Modified_by = AdminUser;
                    objRegistration.Modified_date = DateTime.Now.ToString();
                    result = helper.UpdateUserDetails(objRegistration);
                    ModelState.Clear();
                    return Json(result, JsonRequestBehavior.AllowGet);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult Print()
        {
            DBHelper objdbhandle = new DBHelper();
            List<Registration> RegList = objdbhandle.GetAllUSers();

            var table = new DataTable("Users Information");
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("Designation", typeof(string));
            table.Columns.Add("PhoneNumber", typeof(string));
            
            foreach (Registration registration in RegList)
                table.Rows.Add(registration.Name, registration.UserName, registration.Designation, registration.PhoneNumber);

            var pdf = table.ToPdf();
            System.IO.File.WriteAllBytes(@"G:\project-ust-05-12-2021\Deepak\second\Hospital-Management-System\Hospital Management System\PdfViewer\Users.pdf", pdf);
            return PartialView("_PrintPreview");
        }


        [HttpGet]
        public ActionResult PrintPatients()
        {

            DBHelper objdbhandle = new DBHelper();
            List<Patient> patientList = objdbhandle.GetAll();

            var table = new DataTable("Report");
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("lastName", typeof(string));
            table.Columns.Add("Age", typeof(int));
            table.Columns.Add("Gender", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));
            table.Columns.Add("In-Patient", typeof(bool));


            foreach (Patient patient in patientList)
                table.Rows.Add(patient.Name, patient.LastName, patient.Age, patient.Gender, patient.Date, patient.InPatient);

            var pdf = table.ToPdf();
            System.IO.File.WriteAllBytes(@"G:\project-ust-05-12-2021\Deepak\second\Hospital-Management-System\Hospital Management System\PdfViewer\result.pdf", pdf);
            return PartialView("_PrintPreview");

        }
    }
}
