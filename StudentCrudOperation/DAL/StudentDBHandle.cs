using StudentGradingSystem.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace StudentGradingSystem.DAL
{
    public class StudentDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["studentconn"].ToString();
            con = new SqlConnection(constring);
        }

        public void OpenConnection()
        {
            if (con.State.Equals(ConnectionState.Closed))
            {
                con.Open();
            }
        }

        public void CloseConnection()
        {
            if (con.State.Equals(ConnectionState.Open))
            {
                con.Close();
            }
        }

        public bool AddStudent(Student smodel)
        {
            try { 
            connection();
            SqlCommand cmd = new SqlCommand("AddOrUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            
            cmd.Parameters.AddWithValue("@StdId", smodel.Id);
            cmd.Parameters.AddWithValue("@Regnum", smodel.Regnum);
            cmd.Parameters.AddWithValue("@Name", smodel.Name);
            cmd.Parameters.AddWithValue("@Dob", smodel.Dob);
            cmd.Parameters.AddWithValue("@Standard", smodel.Standard);
            cmd.Parameters.AddWithValue("@Mathematics", smodel.Mathematics);
            cmd.Parameters.AddWithValue("@Physics", smodel.Physics);
            cmd.Parameters.AddWithValue("@Chemistry", smodel.Chemistry);
            cmd.Parameters.AddWithValue("@Grade", smodel.Grade);
            cmd.Parameters.AddWithValue("@Active", 1);
            cmd.Parameters.AddWithValue("@Reqtype", smodel.Reqtype);

            OpenConnection();
            int i = cmd.ExecuteNonQuery();
            CloseConnection();

            if (i >= 1)
                return true;
            else
                return false;
            }
            catch(Exception ex)
            {
                string excep = ex.Message;
                return false;
            }
            finally
            {
                 Console.WriteLine("finally executed");
            }
        }

        public Student GetStudentByReg(string regnum)
        {
            connection();
            Student student = new Student();

            SqlCommand cmd = new SqlCommand("StudentGetByReg", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Regnum", regnum);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            OpenConnection();
            sd.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                student.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                student.Regnum = Convert.ToString(dt.Rows[0]["Regnum"]);
                student.Name = Convert.ToString(dt.Rows[0]["Name"]);
                student.Dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
                student.Standard = Convert.ToString(dt.Rows[0]["Standard"]);
                student.Mathematics = Convert.ToInt32(dt.Rows[0]["Mathematics"]);
                student.Physics = Convert.ToInt32(dt.Rows[0]["Physics"]);
                student.Chemistry = Convert.ToInt32(dt.Rows[0]["Chemistry"]);
                student.Grade = Convert.ToString(dt.Rows[0]["Grade"]);
                dt.Clear();
            }
            else
            {
                return null;
            }           
            CloseConnection();
            return student;
        }

        public Student GetStudentById(int Id)
        {
            connection();
            Student student = new Student();

            SqlCommand cmd = new SqlCommand("StudentGetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            OpenConnection();
            sd.Fill(dt);
            CloseConnection();

            student.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
            student.Regnum = Convert.ToString(dt.Rows[0]["Regnum"]);
            student.Name = Convert.ToString(dt.Rows[0]["Name"]);
            student.Dob = Convert.ToDateTime(dt.Rows[0]["Dob"]);
            student.Standard = Convert.ToString(dt.Rows[0]["Standard"]);
            student.Mathematics = Convert.ToInt32(dt.Rows[0]["Mathematics"]);
            student.Physics = Convert.ToInt32(dt.Rows[0]["Physics"]);
            student.Chemistry = Convert.ToInt32(dt.Rows[0]["Chemistry"]);
            student.Grade = Convert.ToString(dt.Rows[0]["Grade"]);

            return student;
        }

        public List<Student> GetStudent(int offsetValue, int PagingSize, string search)
        {
            connection();
            List<Student> studentlist = new List<Student>();

            SqlCommand cmd = new SqlCommand("StudentGetPagination", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@offsetValue", offsetValue);
            cmd.Parameters.AddWithValue("@PagingSize", PagingSize);
            cmd.Parameters.AddWithValue("@search", search);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            OpenConnection();
            sd.Fill(dt);
            CloseConnection();

            foreach (DataRow dr in dt.Rows)
            {
                studentlist.Add(
                    new Student
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Regnum= Convert.ToString(dr["Regnum"]),
                        Name = Convert.ToString(dr["Name"]),
                        Dob = Convert.ToDateTime(dr["Dob"]),
                        Standard = Convert.ToString(dr["Standard"]),
                        Mathematics = Convert.ToInt32(dr["Mathematics"]),
                        Physics = Convert.ToInt32(dr["Physics"]),
                        Chemistry = Convert.ToInt32(dr["Chemistry"]),
                        Grade = Convert.ToString(dr["Grade"]),
                       
                    });
            }
            return studentlist;
        }


        public string UpdateDetails(Student smodel)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("AddOrUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StdId", smodel.Id);
                cmd.Parameters.AddWithValue("@Regnum", smodel.Regnum);
                cmd.Parameters.AddWithValue("@Name", smodel.Name);
                cmd.Parameters.AddWithValue("@Dob", smodel.Dob);
                cmd.Parameters.AddWithValue("@Standard", smodel.Standard);
                cmd.Parameters.AddWithValue("@Mathematics", smodel.Mathematics);
                cmd.Parameters.AddWithValue("@Physics", smodel.Physics);
                cmd.Parameters.AddWithValue("@Chemistry", smodel.Chemistry);
                cmd.Parameters.AddWithValue("@Grade", smodel.Grade);
                cmd.Parameters.AddWithValue("@Active", 1);
                cmd.Parameters.AddWithValue("@Reqtype", smodel.Reqtype);

                OpenConnection();
                int i = cmd.ExecuteNonQuery();
                CloseConnection();

                if (i >= 1)
                    return "Success";
                else
                    return "Failed";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Console.WriteLine("finally executed");
            }
        }

        public bool DeleteStudent(int id)
        {
            try { 
            connection();
            SqlCommand cmd = new SqlCommand("DeleteStudent", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@StdId", id);

            OpenConnection();
            int i = cmd.ExecuteNonQuery();
            CloseConnection();

            if (i >= 1)
               return true;
            else
               return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Console.WriteLine("finally executed");
            }
        }
    }
}