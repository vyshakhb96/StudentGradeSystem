using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using StudentLibraries.Enum;
using StudentGradingSystem.Models;
using StudentGradingSystem.DAL;

namespace StudentGradingSystem.Controllers
{
    public class StudentController : Controller
    {


        // GET: Student
        public ActionResult Index()
        {
            StudentDBHandle objDBHandle = new StudentDBHandle();
            ModelState.Clear();
            return View();
        }

       
        private string GradeCalculation(int maths, int phy, int che)
        {
            float Total = maths + phy + che;
            float Average = (Total / 300) * 100;
            string Grade;
            if (Average >= 90)
            {
                return Grade = "A+";
            }
            else if (Average >= 85)
            {
                return Grade = "A";
            }
            else if (Average >= 80)
            {
                return Grade = "B+";
            }
            else if (Average >= 75)
            {
                return Grade = "B";
            }
            else if (Average >= 70)
            {
                return Grade = "C+";
            }
            else if (Average >= 65)
            {
                return Grade = "C";
            }
            else if (Average >= 60)
            {
                return Grade = "D+";
            }
            else
            {
                return Grade = "FAILED";
            }

        }

        [HttpPost]
        public ActionResult GetData()
        {          
            StudentDBHandle objDBHandle = new StudentDBHandle();

            var pagenumber = Convert.ToInt32(Request.Form["start"]);
            var pagesize = Convert.ToInt32(Request.Form["length"]);
            var search = Request.Form["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<Student> StudentList = objDBHandle.GetStudent(pagenumber, pagesize, search);
            //int totalrows = StudentList.Count;
            
            //if (!string.IsNullOrEmpty(search))
            //{
            //    StudentList = StudentList.
            //        Where(x => x.Name.ToLower().Contains(search.ToLower()) ||
            //        x.Regnum.ToLower().Contains(search.ToLower()) ||
            //        x.Dob.ToString().Contains(search) ||
            //        x.Standard.ToString().Contains(search.ToLower()) ||
            //        x.Mathematics.ToString().Contains(search.ToLower()) ||
            //        x.Physics.ToString().Contains(search.ToLower()) ||
            //        x.Chemistry.ToString().Contains(search.ToLower()) ||
            //        x.Grade.ToLower().Contains(search.ToLower())).ToList<Student>();
            //}
            //int totalrowsafterfiltering = StudentList.Count;
            StudentList = StudentList.OrderBy(sortColumnName + " " + sortDirection).ToList<Student>();
            StudentList = StudentList.Skip(pagenumber).Take(pagesize).ToList<Student>();
           // return Json(new { data = StudentList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

            return Json(new { data = StudentList }, JsonRequestBehavior.AllowGet);
        }

        // GET: Student/Create
        [HttpGet]
        public ActionResult Create(string operation)
        {
            
            Student objStudent = new Student();
            if (operation == Operations.Add.ToString())
            {
                objStudent.ActionType = Operations.Add;
            }
            ViewData["Standard"] = "";
            return PartialView("_AddViewEdit", objStudent);

        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(Student objStudent)
        {
            try
            {
                int startNumber = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string search = Request["search[value]"];                
                objStudent.Grade = GradeCalculation(objStudent.Mathematics, objStudent.Physics, objStudent.Chemistry);
                StudentDBHandle objDBHandle = new StudentDBHandle();
                var isRegExist = objDBHandle.GetStudentByReg(objStudent.Regnum);
                if (isRegExist == null)
                {
                    if (ModelState.IsValid)
                    {
                      
                        objStudent.Reqtype = "INSERT";
                        var success= objDBHandle.AddStudent(objStudent);
                        if (success)
                        {
                            ViewBag.Message = "Student Details Added Successfully";
                            ModelState.Clear();                           
                        }
                    }
                    else
                    {
                        return PartialView("_AddViewEdit");
                    }
                }
                else
                {
                    ViewBag.Message = "exist!";
                    return PartialView("_AddViewEdit");
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(string operation, int id)
        {
            StudentDBHandle objDBHandle = new StudentDBHandle();
            Student sm = new Student();
            int startNumber = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string search = Request["search[value]"];
            var stud = objDBHandle.GetStudentById(id);
            //var dob = DateTime.Parse(stud.Dob);
            //stud.Dob = dob.ToString("dd-MM-yyyy");
            ViewData["Standard"] = stud.Standard;
            if (operation == Operations.Edit.ToString())
            {
                stud.ActionType = Operations.Edit;
            }
            if(operation == Operations.View.ToString())
            {
                stud.ActionType = Operations.View;
            }
            return PartialView("_AddViewEdit", stud);
    
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Student objStudent)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objStudent.Reqtype = "UPDATE";
                    objStudent.Grade = GradeCalculation(objStudent.Mathematics, objStudent.Physics, objStudent.Chemistry);
                    StudentDBHandle objDBHandle = new StudentDBHandle();
                    objDBHandle.UpdateDetails(objStudent);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return PartialView("_AddViewEdit");
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
            StudentDBHandle objDBHandle = new StudentDBHandle();
            var stud = objDBHandle.GetStudentById(id);
            return PartialView("_Delete", stud);
            
        }
        
        [HttpPost]
        public ActionResult Delete(Student objStudent)
        {
            try
            {
                int id = objStudent.Id;
                StudentDBHandle objDBHandle = new StudentDBHandle();
                if (objDBHandle.DeleteStudent(id))
                {
                    ViewBag.AlertMsg = "Student Deleted Successfully";
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
    }
}
