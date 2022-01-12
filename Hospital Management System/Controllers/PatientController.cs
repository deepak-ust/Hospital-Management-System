using HospitalManagementLibrary;
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

       

        // GET: Patient/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patient/Create
        [HttpPost]
        public ActionResult Create(PatientModel pmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PatientDBHandle pdb = new PatientDBHandle();
                    if (pdb.UpdateDetails(pmodel))
                    {
                        ViewBag.Message = "Patient Details Added Successfully";
                        ModelState.Clear();
                    }
                    return RedirectToAction("Index");
                }
                return View();

            }
            catch
            {
                return View();
            }
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int id)
        {
            PatientDBHandle pdb = new PatientDBHandle();
            return View(pdb.GetById(id));
        }

        // POST: Patient/Edit/5
        [HttpPost]
        public ActionResult Edit(PatientModel pmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PatientDBHandle pdb = new PatientDBHandle();
                    if (pdb.UpdateDetails(pmodel))
                    {
                        ViewBag.Message = "Patient Details Updated Successfully";
                        ModelState.Clear();
                    }
                }
                 return View();
            }
            catch
            {
                return View();
            }
        }
        // GET: Patient/Delete/5
        public ActionResult Delete(int id)
        {
            PatientDBHandle pdb = new PatientDBHandle();
            return View(pdb.GetById(id));
        }
        // POST: Patient/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                PatientDBHandle pdb = new PatientDBHandle();
                if (pdb.DeletePatient(id))
                {
                    ViewBag.AlertMsg = "Patient Deleted Successfully";
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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
                List<PatientModel> patients = pdb.GetPatient(start, length, searchValue);
                return Json(new { data = patients }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }

        }


    }
}
