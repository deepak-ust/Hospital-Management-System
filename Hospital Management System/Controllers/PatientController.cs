using Hospital_Management_System.Models;
using Hospital_Management_System.DAL;
using HospitalManagementLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Linq;
using System.Data;
using ArrayToPdf;

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

        //[HttpGet]
        //public ActionResult Print()
        //{

        //    DBHelper objdbhandle = new DBHelper();
        //    List<Patient> objpatient = objdbhandle.GetAll();

        //    var table = new DataTable("Example Table");
        //    table.Columns.Add("Name", typeof(string));
        //    table.Columns.Add("LastName", typeof(string));
        //    table.Columns.Add("Age", typeof(int));
        //    table.Columns.Add("Gender", typeof(string));
        //    table.Columns.Add("dateTime", typeof(DateTime));
        //    table.Columns.Add("InPatient", typeof(Boolean));


        //    foreach (Patient patient in objpatient)
        //        table.Rows.Add(patient.Name,patient.LastName,patient.Age,patient.Gender, patient.Date,patient.InPatient);

        //    var pdf = table.ToPdf();
        //    System.IO.File.WriteAllBytes(@"C:\Users\91623\Desktop\Assignment\Deepak\Hospital-Management-System\Hospital Management System\PdfViewer\report.pdf", pdf);
        //    return PartialView("_Operations");

        //}

        [HttpGet]
        public ActionResult Print()
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
            System.IO.File.WriteAllBytes(@"D:\Assingnment\Deepak\Hospital-Management-System\Hospital Management System\PdfViewer\result.pdf", pdf);
            return PartialView("_PrintPreview");

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
                string columnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string direction = Request["order[0][dir]"];
                List<Patient> patients = helper.GetData(start, length, searchValue, columnName, direction);
                patients = patients.OrderBy(columnName + " " + direction).ToList();
                int count = helper.GetFullCount(searchValue);
                return Json(new { data = patients,recordsTotal=count,recordsFiltered=count }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
    }
}
