using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using StudentLibraries.Enum;
using StudentGradingSystem.Models;

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
       

        public string GradeCalculation(int maths, int phy, int che)
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
            int startNumber = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);
            string searchKey = Convert.ToString(Request.Form["search[value]"]);
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];

            List<Student> StudentList = new List<Student>();
            StudentDBHandle db = new StudentDBHandle();


                StudentList = db.GetStudent(startNumber,length,searchKey);
                int totalrows = StudentList.Count;
                for (int i = 0; i < StudentList.Count; i++)
                {
                    var dob = DateTime.Parse(StudentList[i].Dob);
                StudentList[i].Dob = dob.ToString("dd-MM-yyyy");
                }
                if (!string.IsNullOrEmpty(searchKey))
                {
                    StudentList = StudentList.
                        Where(x => x.Name.ToLower().Contains(searchKey.ToLower()) || 
                        x.Regnum.ToLower().Contains(searchKey.ToLower()) || 
                        x.Dob.Contains(searchKey) || 
                        x.Standard.ToString().Contains(searchKey.ToLower()) || 
                        x.Mathematics.ToString().Contains(searchKey.ToLower()) || 
                        x.Physics.ToString().Contains(searchKey.ToLower()) || 
                        x.Chemistry.ToString().Contains(searchKey.ToLower()) || 
                        x.Grade.ToLower().Contains(searchKey.ToLower())).ToList<Student>();
                }
                int totalrowsafterfiltering = StudentList.Count;
                //sorting
                StudentList = StudentList.OrderBy(sortColumnName + " " + sortDirection).ToList<Student>();
                //paging
                StudentList = StudentList.Skip(startNumber).Take(length).ToList<Student>();

                return Json(new { data = StudentList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            
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
                
                ViewData["Standard"] = "";
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
