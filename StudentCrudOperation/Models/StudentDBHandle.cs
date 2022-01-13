using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;


namespace StudentCrudOperation.Models
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

        public bool AddStudent(StudentModel smodel)
        {
            try { 
            connection();
            SqlCommand cmd = new SqlCommand("AddNewStudent", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Regnum", smodel.Regnum);
            cmd.Parameters.AddWithValue("@Name", smodel.Name);
            cmd.Parameters.AddWithValue("@Dob", smodel.Dob);
            cmd.Parameters.AddWithValue("@Standard", smodel.Standard);
            cmd.Parameters.AddWithValue("@Mathematics", smodel.Mathematics);
            cmd.Parameters.AddWithValue("@Physics", smodel.Physics);
            cmd.Parameters.AddWithValue("@Chemistry", smodel.Chemistry);
            cmd.Parameters.AddWithValue("@Grade", smodel.Grade);
            cmd.Parameters.AddWithValue("@Active", 1);

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
                return false;
            }
            finally
            {
                 Console.WriteLine("finally executed");
            }
        }

        public List<StudentModel> GetStudent()
        {
            connection();
            List<StudentModel> studentlist = new List<StudentModel>();

            SqlCommand cmd = new SqlCommand("GetStudentDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            OpenConnection();
            sd.Fill(dt);
            CloseConnection();

            foreach (DataRow dr in dt.Rows)
            {
                studentlist.Add(
                    new StudentModel
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Regnum= Convert.ToString(dr["Regnum"]),
                        Name = Convert.ToString(dr["Name"]),
                        Dob = Convert.ToString(dr["Dob"]),
                        Standard = Convert.ToString(dr["Standard"]),
                        Mathematics = Convert.ToInt32(dr["Mathematics"]),
                        Physics = Convert.ToInt32(dr["Physics"]),
                        Chemistry = Convert.ToInt32(dr["Chemistry"]),
                        Grade = Convert.ToString(dr["Grade"]),
                        Active = Convert.ToInt32(dr["Active"])
                    });
            }
            return studentlist;
        }


        public string UpdateDetails(StudentModel smodel)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("UpdateStudentDetails", con);
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
            catch (Exception ex)
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