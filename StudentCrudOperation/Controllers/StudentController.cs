using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVCLibraries;
using System.Linq.Dynamic;

namespace StudentCrudOperation.Controllers
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

        // GET: Student/Create
        public ActionResult Create()
        {
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

            List<StudentModel> StudentList = new List<StudentModel>();
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
                        x.Grade.ToLower().Contains(searchKey.ToLower())).ToList<StudentModel>();
                }
                int totalrowsafterfiltering = StudentList.Count;
                //sorting
                StudentList = StudentList.OrderBy(sortColumnName + " " + sortDirection).ToList<StudentModel>();
                //paging
                StudentList = StudentList.Skip(startNumber).Take(length).ToList<StudentModel>();

                return Json(new { data = StudentList, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
            
        }


        // POST: Student/Create
        [HttpPost]
        public ActionResult Create(StudentModel smodel)
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
                        StudentDBHandle sdb = new StudentDBHandle();
                        if (sdb.AddStudent(smodel))
                        {
                            ViewBag.Message = "Student Details Added Successfully";
                            ModelState.Clear();
                            return RedirectToAction("Index", "Student");
                        }
                    }
                }
                else
                {
                    ViewBag.Message = "exist!";

                }

                return View();
            }
            catch(Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
        }

        // GET: Student/Edit/5
        public ActionResult Edit(int id)
        {        
                StudentDBHandle sdb = new StudentDBHandle();
                int startNumber = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchKey = Request["search[value]"];
                var stud = sdb.GetStudentById(id);
                var dob = DateTime.Parse(stud.Dob);
                stud.Dob = dob.ToString("dd-MM-yyyy");
                ViewData["Standard"] = stud.Standard;
                return View(stud);
    
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, StudentModel smodel)
        {
            try
            {
                smodel.Grade = GradeCalculation(smodel.Mathematics, smodel.Physics, smodel.Chemistry);
                StudentDBHandle sdb = new StudentDBHandle();
                sdb.UpdateDetails(smodel);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
                return View();
            }
            finally
            {
                ViewData["FinalError"] = "Error occured!";
            }
        }

        // GET: Student/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                StudentDBHandle sdb = new StudentDBHandle();
                if (sdb.DeleteStudent(id))
                {
                    ViewBag.AlertMsg = "Student Deleted Successfully";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }
            finally
            {
                ViewData["FinalError"] = "Error occured!";
            }
            return View();
        }
    }
}
