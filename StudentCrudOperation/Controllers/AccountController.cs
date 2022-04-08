using StudentCrudOperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using StudentCrudOperation.DAL;

namespace StudentCrudOperation.Controllers
{
    public class AccountController : Controller
    {
        
        // GET:Login this Action method simple return the Login View
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            UserDBHandle db = new UserDBHandle();
            if (User.Identity.IsAuthenticated)
            {
                var user = db.GetUserbyMail(User.Identity.Name);
                if (user.Role == "Admin")
                {
                    return RedirectToAction("StudentView", "User");
                }
                else
                {
                    return RedirectToAction("Index", "Student");

                }
            }

            ViewBag.ReturnUrl = returnUrl ?? Url.Action("Index", "Student");
            return View();
        }
        //Post:When user click on the submit button then this method will call
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login LoginViewModel)
        {
            UserDBHandle db = new UserDBHandle();
            var user = db.IsValidUser(LoginViewModel.Email, LoginViewModel.Password);
            if (user!=null)
            {
                FormsAuthentication.SetAuthCookie(LoginViewModel.Email, false);
                if (user.Role == "Admin")
                {
                    return RedirectToAction("StudentView", "User");
                }
                else
                {
                return RedirectToAction("Index", "Student");
                }
                
            }
            else
            {
                ModelState.AddModelError("", "Your Email and password doesn't match");
            }
            return View(LoginViewModel);
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(User userObj)
        {
            UserDBHandle db = new UserDBHandle();
            var user = db.ResetCheck(userObj.Email, userObj.Dob);
            if (user != null)
            {
                var reset = db.ResetPassword(userObj.Email,userObj.Password,user.Id);
                if (reset == "Success")
                {
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    return HttpNotFound();
                }

            }
            else
            {
                ModelState.AddModelError("", "Your Email and DOB doesn't match");
            }
            return View();
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}