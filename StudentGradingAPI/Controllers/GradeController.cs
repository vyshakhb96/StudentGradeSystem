using StudentLibraries;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Http;

namespace StudentGradingAPI.Controllers
{
    [RoutePrefix("api/Grade")]
    public class Grades : ApiController
    {    
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


        // GET: api/Grade
        public string Get(int offsetValue, int PagingSize, string search)
        {
            StudentDBHandle sdb = new StudentDBHandle();
            var stud = sdb.GetStudent(offsetValue, PagingSize, search);
           
            if (stud != null)
            {
                return JsonConvert.SerializeObject(stud);
            }
            else
            {
                return "No data found!";
            }
        }

        // GET: api/Grade/5
        public string GetByID(int id)
        {
            StudentDBHandle sdb = new StudentDBHandle();
            var stud = sdb.GetStudentById(id);
            var dob = DateTime.Parse(stud.Dob);
            stud.Dob = dob.ToString("dd-MM-yyyy");
           
            if (stud != null)
            {
                return JsonConvert.SerializeObject(stud);
            }
            else
            {
                return "No data found!";
            }          
        }


        [HttpPost]
        [Route("Create")]
        public string Create([FromBody] Student smodel)
        {
           
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
                            return  "Student Details Added Successfully";                          
                          
                        }
                        else
                        {
                            return "Student Details not added";
                        }
                    }
                    else
                    {
                        return "Instance of Model state is not valid!";
                    }
                }
                else
                {
                    return "Student Already exist with this Register number!";
                }

        }


        [HttpPut]
        [Route("Edit")]
        public string Edit(int id, Student smodel)
        {
            try
            {
                smodel.Id = id;
                smodel.Grade = GradeCalculation(smodel.Mathematics, smodel.Physics, smodel.Chemistry);
                StudentDBHandle sdb = new StudentDBHandle();
                return sdb.UpdateDetails(smodel);
            }
            catch (Exception ex)
            {
                return ex.Message;
                
            }
           
        }

        
        [HttpDelete]
        [Route("Delete")]
        public string Delete(int id)
        {
            try
            {
                StudentDBHandle sdb = new StudentDBHandle();
                if (sdb.DeleteStudent(id))
                {
                   return "Student Deleted Successfully";
                }
                else
                {
                    return "Student not Deleted!";
                }
               
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

    }
}
