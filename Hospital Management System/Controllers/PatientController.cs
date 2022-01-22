using Hospital_Management_System.Models;
using Hospital_Management_System.DAL;
using HospitalManagementLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Web.Mvc;


namespace Hospital_Management_System.Controllers
{
    public class PatientController : Controller
    {
       
        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

       
        // POST: Patient/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Patient objPatient)
        {
            bool result = false;
            DBHelper helper = new DBHelper();
            try
            {
                if (ModelState.IsValid)
                {
                    result = helper.UpdateDetails(objPatient);
                    ModelState.Clear();
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                    return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        // GET: Patient/Get/5
        public ActionResult Get(string operation, int id)
        {
            if (id > 0)
            {
                DBHelper helper = new DBHelper();
                Patient objPatient = helper.GetById(id);
                ViewData["Gender"] = objPatient.Gender;
                if (operation == Operations.View.ToString())
                {
                    objPatient.ActionType = Operations.View;
                    return PartialView("_Operations", objPatient);
                }
                else if (operation == Operations.Edit.ToString())
                {
                    objPatient.ActionType = Operations.Edit;
                    return PartialView("_Operations", objPatient);
                }
                else if (operation == Operations.Delete.ToString())
                {
                    objPatient.ActionType = Operations.Edit;
                    ViewData["Name"] = objPatient.Name;
                    return PartialView("_Delete", objPatient);
                }
                else
                    return View();
            }
            else
            {
                Patient objPatient = new Patient
                {
                    ActionType = Operations.Add
                };
                ViewData["Gender"] = "Select";
                return PartialView("_Operations", objPatient);
            }
        }

        
        
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            try
            {
                DBHelper helper = new DBHelper();
                result = helper.DeletePatient(id);
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       //GET: Patient/PatientData
        [HttpPost]
        public ActionResult PatientData()
        {
            try
            {
                DBHelper helper = new DBHelper();

                var start = Convert.ToInt32(Request.Form["start"]);
                var length = Convert.ToInt32(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];
                List<Patient> patients = helper.GetPatient(start, length, searchValue);
                return Json(new { data = patients }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }


    }
}
