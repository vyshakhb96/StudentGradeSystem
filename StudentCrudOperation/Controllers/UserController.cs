using StudentCrudOperation.DAL;
using StudentCrudOperation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using StudentLibraries.Enum;

namespace StudentCrudOperation.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Create(string operation)
        {

            User objUser = new User();
            if (operation == Operations.Add.ToString())
            {
                objUser.ActionType = Operations.Add;
            }
            return PartialView("_UserOperations", objUser);
        }
        [HttpPost]
        public ActionResult Create(User objUser)
        {
            try
            {
                
                UserDBHandle objDBHandle = new UserDBHandle();
                        objUser.Reqtype = "INSERT";

                if (ModelState.IsValid)
                    {

                        var success = objDBHandle.AddUser(objUser);
                        if (success)
                        {
                            ViewBag.Message = "User Details Added Successfully";
                            ModelState.Clear();
                        }
                    }
                    else
                    {
                        return PartialView("_UserOperations");
                    }
                
               
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult GetUsers()
        {
            try
            {
                UserDBHandle objDBHandle = new UserDBHandle();
                var PageNumber = Convert.ToInt32(Request.Form["start"]);
                var RowsInPage = Convert.ToInt32(Request.Form["length"]);
                var SearchValue = Convert.ToString(Request.Form["search[value]"]);
                string sortColumnName = Request.Form["columns[" + Request["order[0][column]"] + "][data]"];
                string sortDirection = Request["order[0][dir]"];
                List<User> userList = objDBHandle.GetUser(PageNumber, RowsInPage, SearchValue);
                userList = userList.OrderBy(sortColumnName + " " + sortDirection).ToList<User>();

                int totalCount = objDBHandle.GetCount(SearchValue);
                return Json(new
                {
                    data = userList,
                    recordsTotal = totalCount,
                    recordsFiltered = totalCount
                }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
    }
}