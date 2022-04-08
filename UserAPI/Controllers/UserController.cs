using StudentCrudOperation.DAL;
using StudentCrudOperation.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace UserAPI.Controllers
{
    public class UserController : ApiController
    {
        [HttpPost]
        [ActionName("Create")]
        public IHttpActionResult Create(User objUser)
        {
            try
            {

                UserDBHandle objDBHandle = new UserDBHandle();
                objUser.Reqtype = "INSERT";
                
                objUser.CreatedDate = DateTime.Now.ToString();
                


                if (ModelState.IsValid)
                {
                    var user = objDBHandle.IsUserExist(objUser.Email);
                    if (user == null)
                    {
                        var success = objDBHandle.AddUser(objUser);
                        if (success)
                        {
                            ModelState.Clear();
                            return Created("", "Created successfully");
                        }
                        else
                        {
                           return InternalServerError();
                        }
                    }
                    else
                    {
                        return BadRequest("User with email already exists");
                    }
                }
                else
                {
                    return BadRequest("Invalid model");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [ActionName("GetById")]
        public IHttpActionResult GetById(int id)
        {
            UserDBHandle db = new UserDBHandle();
            User objUser = new User();
            var user = db.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPut]
        [ActionName("Edit")]
        public IHttpActionResult Edit(User objUser)
        {
            try
            {
                UserDBHandle objDBHandle = new UserDBHandle();
                
                objUser.ModifiedDate = DateTime.Now.ToString();
                if (ModelState.IsValid)
                {
                    objUser.Reqtype = "UPDATE";
                    var msg=objDBHandle.UpdateDetails(objUser);
                    if (msg == "Success")
                    {
                        return Created("", "Updated successfully");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest("Invalid model");
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        

        [HttpDelete]
        [ActionName("Delete")]
        public IHttpActionResult Delete(int id)
        {
            try
            {
                UserDBHandle objDBHandle = new UserDBHandle();
                if (objDBHandle.DeleteUser(id))
                {
                    return Created("", "Deleted successfully");
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [ActionName("GetUsers")]
        public IHttpActionResult GetUsers(int PageNumber,int RowsInPage,string SearchValue)
        {
            try
            {
                UserDBHandle objDBHandle = new UserDBHandle();
                
                List<User> userList = objDBHandle.GetUser(PageNumber, RowsInPage, SearchValue);
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
