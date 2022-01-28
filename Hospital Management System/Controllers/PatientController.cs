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
       
        // POST: Patient/Update/obj
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Patient obj)
        {
            bool result = false;
            DBHelper helper = new DBHelper();
            try
            {
                if (ModelState.IsValid)
                {
                    result = helper.UpdateDetails(obj);
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

        // GET: Patient/Get?operation&id
        public ActionResult Get(string operation, int id)
        {
            if (id > 0)
            {
                DBHelper helper = new DBHelper();
                Patient obj = helper.GetById(id);
                if (operation == Operations.View.ToString())
                {
                    ViewData["Gender"] = obj.Gender;
                    ViewData["Date"] = obj.Date;
                    obj.ActionType = Operations.View;
                    return PartialView("_Operations", obj);
                }
                else
                {
                    ViewData["Name"] = obj.Name;
                    return PartialView("_Delete", obj);
                }
            }
            else
            {
                Patient obj = new Patient 
                {
                    ActionType = Operations.Add
                };
                ViewData["Gender"] = "Select";
                ViewData["Date"] = DateTime.Today;
                return PartialView("_Operations", obj);
            }
        }

        //POST: Patient/Delete/id
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            try
            {
                DBHelper helper = new DBHelper();
                result = helper.DeleteData(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       //POST: Patient/PatientData
        [HttpPost]
        public ActionResult PatientData()
        {
            try
            {
                DBHelper helper = new DBHelper();

                var start = Convert.ToInt32(Request.Form["start"]);
                var length = Convert.ToInt32(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];
                List<Patient> patients = helper.GetData(start, length, searchValue);
                return Json(new { data = patients }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
    }
}
