using StudentCrudOperation.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace StudentCrudOperation.DAL
{
    public class UserDBHandle
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

        public bool AddUser(User userobj)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("AddOrUpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", userobj.Id);
                cmd.Parameters.AddWithValue("@Username", userobj.UserName);
                cmd.Parameters.AddWithValue("@Email", userobj.Email);
                cmd.Parameters.AddWithValue("@Password", userobj.Password);
                cmd.Parameters.AddWithValue("@Reqtype", userobj.Reqtype);
                cmd.Parameters.AddWithValue("@Active", userobj.Active);




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
                string excep = ex.Message;
                return false;
            }
            finally
            {
                Console.WriteLine("finally executed");
            }
        }

        public int GetCount(string search)
        {
            int count = 0;
            connection();
            SqlCommand cmd = new SqlCommand("GetUserCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@search", search);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                count = Convert.ToInt32(row["Column1"]);
            }
            return count;
        }


        public User GetUserById(int id)
        {
            connection();
            User user = new User();

            SqlCommand cmd = new SqlCommand("StudentGetByReg", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            OpenConnection();
            sd.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                user.UserName = Convert.ToString(dt.Rows[0]["Username"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                
                dt.Clear();
            }
            else
            {
                return null;
            }
            CloseConnection();
            return user;
        }

       

        public List<User> GetUser(int offsetValue, int PagingSize, string search)
        {
            connection();
            List<User> userList = new List<User>();

            SqlCommand cmd = new SqlCommand("GetAllUsers", con);
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
                userList.Add(
                    new User
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        UserName = Convert.ToString(dr["Username"]),
                        Email = Convert.ToString(dr["Email"]),
                        Password = Convert.ToString(dr["Password"]),

                    });
            }
            return userList;
        }


        public string UpdateDetails(User userobj)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("AddOrUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", userobj.Id);
                cmd.Parameters.AddWithValue("@Username", userobj.UserName);
                cmd.Parameters.AddWithValue("@Email", userobj.Email);
                cmd.Parameters.AddWithValue("@Password", userobj.Password);
               

                OpenConnection();
                int i = cmd.ExecuteNonQuery();
                CloseConnection();

                if (i >= 1)
                    return "Success";
                else
                    return "Failed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                Console.WriteLine("finally executed");
            }
        }

        public bool DeleteUser(int id)
        {
            try
            {
                connection();
                SqlCommand cmd = new SqlCommand("DeleteStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

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

        public List<User> ListOfUsers()
        {
            connection();
            List<User> userList = new List<User>();

            SqlCommand cmd = new SqlCommand("GetAllStudent", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                userList.Add(
                    new User
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        UserName = Convert.ToString(dr["Username"]),
                        Email = Convert.ToString(dr["Email"]),
                        Password = Convert.ToString(dr["Password"]),
                       
                    });



            }


            return userList;
        }
    }
}