using Hospital_Management_System.DAL;
using Hospital_Management_System.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Routing;
using System.Web.Security;
using System.Windows.Forms;

namespace Hospital_Management_WebAPI.Controllers
{
    [RoutePrefix("api/Employee")]
    public class EmployeeController : ApiController
    {
        [HttpPost]
        [Route("CreateUsers")]
        public string CreateUsers(Registration registration)
        {
          
            var AdminUser = User.Identity.Name;
            DBHelper helper = new DBHelper();
            registration.Created_by = 1;
            registration.Created_date = DateTime.Now.ToString();
            registration.Modified_by = 1;
            registration.Modified_date = DateTime.Now.ToString();
            var isRegExist = helper.GetByUsername(registration.Email);
            if (isRegExist.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    var success = helper.AddUser(registration);
                    if (success)
                    {
                        return "Registration Successfull";
                      
                    }
                }
                else
                {
                    return "Registration UnSuccessfull";
                }
            }
            else
            {
                return "User Exist";
            }
            return "Sucsess";
        }


        [HttpGet]
        [Route("GetData")]
        public string GetData(int start, int length, string searchValue)
        {
            try
            {
                DBHelper helper = new DBHelper();              
                List<Registration> registeredData = helper.GetReg(start, length, searchValue);
                int count = helper.GetFullCountOfUsers(searchValue);
                return JsonConvert.SerializeObject(registeredData);
            }
            catch (Exception ex)
            {
                string excep = ex.Message;
                return excep;
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public string Delete(int id)
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
            return JsonConvert.SerializeObject(result);           
        }

        [HttpPut]
        [Route("UpdateUser")]
        public string UpdateUser([FromBody] Registration objRegistration)
        {
            bool result = false;
            DBHelper helper = new DBHelper();
            try
            {
                var dataresult = helper.GetByIdOfUser(objRegistration.Id);
                var AdminUser = User.Identity.Name;
                objRegistration.Password = dataresult.Password;
                objRegistration.IsAdmin = dataresult.IsAdmin;
                objRegistration.Created_by = dataresult.Created_by;
                objRegistration.Created_date = dataresult.Created_date;
                objRegistration.Modified_by = 1;
                objRegistration.Modified_date = DateTime.Now.ToString();
                result = helper.UpdateUserDetails(objRegistration);
                ModelState.Clear();
                return JsonConvert.SerializeObject(result);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("Get")]
        public string Get(int id)
        {           
            DBHelper helper = new DBHelper();
            Registration objRegistration = helper.GetByIdOfUser(id);
            return JsonConvert.SerializeObject(objRegistration);
        }
    }
}

