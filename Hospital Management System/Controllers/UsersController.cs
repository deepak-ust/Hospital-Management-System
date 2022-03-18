using Hospital_Management_System.DAL;
using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

            DBHelper helper = new DBHelper();
            Registration objRegister = new Registration();
            var isRegExist = helper.GetByUsername(registration.UserName);
            if (isRegExist.Id == 0)
            {
                if (ModelState.IsValid)
                {
                    var success = helper.AddUser(objRegister);
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
            return Json(true, JsonRequestBehavior.AllowGet);
 
        }


    }
}
