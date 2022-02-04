using Hospital_Management_System.Models;
using Hospital_Management_System.DAL;
using HospitalManagementLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Linq;

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

        // GET: Patient/Get?operation&id
        public ActionResult Get(string operation, int id)
        {
            if (id > 0)
            {
                DBHelper helper = new DBHelper();
                Patient objPatient = helper.GetById(id);
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
                else
                {
                    return PartialView("_Delete", objPatient);
                }
            }
            else
            {
                Patient objPatient = new Patient 
                {
                    ActionType = Operations.Add
                };
                return PartialView("_Operations", objPatient);
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
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][data]"];
                string sortDirection = Request["order[0][dir]"];
                List<Patient> patients = helper.GetData(start, length, searchValue);
                patients = patients.OrderBy(sortColumnName + " " + sortDirection).ToList();
                return Json(new { data = patients }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
    }
}
