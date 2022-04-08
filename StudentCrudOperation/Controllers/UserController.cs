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
    [Authorize(Roles="Admin")]
    public class UserController : Controller
    {
        // GET: User
        public ActionResult UserView()
        {
            return View();
        }
        public ActionResult StudentView()
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
                string createdName = User.Identity.Name;
                User loggedUser = objDBHandle.GetUserbyMail(createdName);
                objUser.CreatedBy = loggedUser.Id;
                objUser.CreatedDate = DateTime.Now.ToString();
                //objUser.ModifiedBy = User.Identity.Name.Split('@')[0];
                //objUser.ModifiedDate = DateTime.Now.ToString();


                if (ModelState.IsValid)
                    {
                    var user = objDBHandle.IsUserExist(objUser.Email);
                    if (user==null)
                    {
                        var success = objDBHandle.AddUser(objUser);
                        if (success)
                        {
                            ViewBag.Message = "User Details Added Successfully";
                            ModelState.Clear();
                        }
                        else
                        {
                            ModelState.AddModelError("", "something went wrong try later!");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User with same email already exists!");
                        return PartialView("_UserOperations");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Data is not correct");
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
        public ActionResult Edit(string operation, int id)
        {
            UserDBHandle db = new UserDBHandle();
            User objUser = new User();
            var user = db.GetUserById(id);

            if (operation == Operations.Edit.ToString())
            {
                user.ActionType = Operations.Edit;
            }
            if (operation == Operations.View.ToString())
            {
                user.ActionType = Operations.View;
            }
            return PartialView("_UserOperations", user);

        }
        [HttpPost]
        public ActionResult Edit(int id, User objUser)
        {
            try
            {
                UserDBHandle objDBHandle = new UserDBHandle();
                string modifiedName = User.Identity.Name;
                User loggedUser = objDBHandle.GetUserbyMail(modifiedName);
                objUser.ModifiedBy = loggedUser.Id;
                objUser.ModifiedDate = DateTime.Now.ToString();
                if (ModelState.IsValid)
                {
                    objUser.Reqtype = "UPDATE";
                    objDBHandle.UpdateDetails(objUser);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return PartialView("_UserOperations");
                }

            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                ViewData["Final"] = "Final excecuted!";
            }
        }
        [HttpGet]
        public ActionResult Delete(string operation, int id)
        {
            UserDBHandle objDBHandle = new UserDBHandle();
            var user = objDBHandle.GetUserById(id);
            return PartialView("_DeleteUser", user);

        }

        [HttpPost]
        public ActionResult Delete(User userObj)
        {
            try
            {
                int id = userObj.Id;
                UserDBHandle objDBHandle = new UserDBHandle();
                if (objDBHandle.DeleteUser(id))
                {
                    ViewBag.AlertMsg = "User Deleted Successfully";
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }
            finally
            {
                ViewData["Final"] = "Done!";
            }
            return View();
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
            catch(Exception ex)
            {
                return View();
            }
        }
    }
}