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
            StudentDBHandle dbhandle = new StudentDBHandle();
            ModelState.Clear();
            return View();
        }

        string Grade;
        private string GradeCalculation(int maths, int phy, int che)
        {
            float Total = maths + phy + che;
            float Average = (Total / 300) * 100;
            
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
            StudentDBHandle obj = new StudentDBHandle();

            var pagenumber = Convert.ToInt32(Request.Form["start"]);
            var pagesize = Convert.ToInt32(Request.Form["length"]);
            var search = Request.Form["search[value]"];

            List<Student> StudentList = obj.GetStudent(pagenumber, pagesize, search);
            for (int i = 0; i < StudentList.Count; i++)
            {
                var dob = DateTime.Parse(StudentList[i].Dob);
                StudentList[i].Dob = dob.ToString("dd-MM-yyyy");
            }
            return Json(new { data = StudentList }, JsonRequestBehavior.AllowGet);
        }

        // GET: Student/Create
        [HttpGet]
        public ActionResult Create(string operation)
        {
            Student sm = new Student();           
            if(operation == Operations.Add.ToString())
            {
                 sm.ActionType = Operations.Add;
            }
            ViewData["Standard"] = "";
            return PartialView("_AddViewEdit", sm);

        }

        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(Student smodel)
        {
            try
            {
                int startNumber = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchKey = Request["search[value]"];                
                smodel.Grade = GradeCalculation(smodel.Mathematics, smodel.Physics, smodel.Chemistry);
                StudentDBHandle dbhandle = new StudentDBHandle();
                var isRegExist = dbhandle.GetStudentByReg(smodel.Regnum);
                if (isRegExist == null)
                {
                    if (ModelState.IsValid)
                    {
                      
                        smodel.Reqtype = "INSERT";
                        var success= dbhandle.AddStudent(smodel);
                        if (success)
                        {
                            ViewBag.Message = "Student Details Added Successfully";
                            ModelState.Clear();                           
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "exist!";
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
            StudentDBHandle sdb = new StudentDBHandle();
            Student sm = new Student();
            int startNumber = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchKey = Request["search[value]"];
            var stud = sdb.GetStudentById(id);
            var dob = DateTime.Parse(stud.Dob);
            stud.Dob = dob.ToString("dd-MM-yyyy");
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
        [HandleError]
        public ActionResult Edit(int id, Student smodel)
        {
            try
            {
                smodel.Reqtype = "UPDATE";
                smodel.Grade = GradeCalculation(smodel.Mathematics, smodel.Physics, smodel.Chemistry);
                StudentDBHandle sdb = new StudentDBHandle();
                sdb.UpdateDetails(smodel);
                return Json(true, JsonRequestBehavior.AllowGet);

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
            StudentDBHandle sdb = new StudentDBHandle();
            var stud = sdb.GetStudentById(id);
            return PartialView("_Delete", stud);
            
        }
        
        [HttpPost]
        public ActionResult Delete(Student smodel)
        {
            try
            {
                int id = smodel.Id;
                StudentDBHandle sdb = new StudentDBHandle();
                if (sdb.DeleteStudent(id))
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
