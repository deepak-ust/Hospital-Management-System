using HospitalManagementLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult Update(Patient pmodel)
        {
            bool result = false;
            PatientDBHandle pdb = new PatientDBHandle();
            try
            {
                if (ModelState.IsValid)
                {
                    result = pdb.UpdateDetails(pmodel);
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
        public JsonResult Get(int id)
        {
            PatientDBHandle pdb = new PatientDBHandle();
            Patient model = pdb.GetById(id);
            string value = string.Empty;
            value = JsonConvert.SerializeObject(model, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            return Json(value, JsonRequestBehavior.AllowGet);
        }

        
        
        [HttpPost]
        public JsonResult Delete(int id)
        {
            bool result = false;
            try
            {
                PatientDBHandle pdb = new PatientDBHandle();
                result = pdb.DeletePatient(id);
                
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
                PatientDBHandle pdb = new PatientDBHandle();

                var start = Convert.ToInt32(Request.Form["start"]);
                var length = Convert.ToInt32(Request.Form["length"]);
                var searchValue = Request.Form["search[value]"];
                List<Patient> patients = pdb.GetPatient(start, length, searchValue);
                return Json(new { data = patients }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }


    }
}
