using HospitalManagementLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HospitalManagementApi.Controllers
{
    public class PatientsController : ApiController
    {
        [HttpGet]
        [ActionName("GetPatientByID")]

        public IHttpActionResult GetById(int id)
        {
            PatientDBHandle handle = new PatientDBHandle();
            var model = handle.GetById(id);
            if(model==null)
            {
                return NotFound();
            }
            return Ok(model);
        }
        [HttpGet]
        [ActionName("GetPatient")]
        public IEnumerable<PatientModel> GetAllPatients(int pageIndex, int pageSize, string searchBy)
        {
            try
            {
                PatientDBHandle handle = new PatientDBHandle();
                List<PatientModel> list = (handle.GetPatient(pageIndex, pageSize, searchBy));
                return list;
            }
            catch
            {
                throw;
            }
        }
        [HttpPost]
        public IHttpActionResult AddPatient(PatientModel patient)
        {
            try
            {
                PatientDBHandle handle = new PatientDBHandle();
                handle.UpdateDetails(patient);
                return Created("","Created successfully");

            }
            catch
            {
                throw;
            }


        }
        [HttpDelete]
        [ActionName("DeletePatient")]
        public IHttpActionResult DeletePatientByID(int id)
        {
            try
            {
                PatientDBHandle handle = new PatientDBHandle();
                handle.DeletePatient(id);
                return Created("", "Deleted successfully");

            }
            catch
            {
                throw;
            }
        }
        [HttpPut]
        [ActionName("UpdatePatient")]
        public IHttpActionResult UpdatePatientByID(PatientModel patient)
        {
            try
            {
                PatientDBHandle handle = new PatientDBHandle();
                handle.UpdateDetails(patient);
                return Created("", "Updated successfully");

            }
            catch
            {
                throw;
            }
        }
    }
}
