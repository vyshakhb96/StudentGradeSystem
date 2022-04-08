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
                var encryptpassword = Base64Encode(userobj.Password);

                connection();
                SqlCommand cmd = new SqlCommand("AddOrUpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", userobj.Id);
                cmd.Parameters.AddWithValue("@Username", userobj.UserName);
                cmd.Parameters.AddWithValue("@Email", userobj.Email);
                cmd.Parameters.AddWithValue("@Password", encryptpassword);
                cmd.Parameters.AddWithValue("@Dob", userobj.Dob);
                cmd.Parameters.AddWithValue("@CreatedBy", userobj.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate", userobj.CreatedDate);
                cmd.Parameters.AddWithValue("@ModifiedBy", userobj.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", userobj.ModifiedDate);
                cmd.Parameters.AddWithValue("@Reqtype", userobj.Reqtype);
                cmd.Parameters.AddWithValue("@IsAdmin", userobj.IsAdmin);

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

            SqlCommand cmd = new SqlCommand("GetUserById", con);
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
                user.Password = Base64Decode(Convert.ToString(dt.Rows[0]["Password"]));
                user.CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]);
                user.CreatedDate = Convert.ToString(dt.Rows[0]["CreatedDate"]);
                user.ModifiedBy = Convert.ToInt32(dt.Rows[0]["ModifiedBy"]);
                user.ModifiedDate = Convert.ToString(dt.Rows[0]["ModifiedDate"]);
                user.Role= Convert.ToString(dt.Rows[0]["Role"]);

                dt.Clear();
            }
            else
            {
                return null;
            }
            CloseConnection();
            return user;
        }
        public User GetUserbyMail(string mail)
        {
            connection();
            User user = new User();

            SqlCommand cmd = new SqlCommand("GetUserByMail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", mail);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            OpenConnection();
            sd.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                user.UserName = Convert.ToString(dt.Rows[0]["Username"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Password = Base64Decode(Convert.ToString(dt.Rows[0]["Password"]));
                user.Dob = Convert.ToString(dt.Rows[0]["Dob"]);
                user.CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]);
                user.CreatedDate = Convert.ToString(dt.Rows[0]["CreatedDate"]);
                user.Role = Convert.ToString(dt.Rows[0]["Role"]);

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
                        CreatedBy= Convert.ToInt32(dr["CreatedBy"]),
                        CreatedDate = Convert.ToString(dr["CreatedDate"]),
                        ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                        ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                        Role = Convert.ToString(dr["Role"])
                    });
               
            }
            foreach(var n in userList)
            {
                if (n.CreatedBy > 0)
                {
                    var user = GetUserById(n.CreatedBy);
                    n.CreatedByName = user.Email.Split('@')[0];
                }
                else
                {
                    n.CreatedByName = "Admin";
                }
                if (n.ModifiedBy > 0)
                {
                    var user = GetUserById(n.ModifiedBy);
                    n.ModifiedByName = user.Email.Split('@')[0];
                }
                else 
                {
                    n.ModifiedByName = "Not modified yet";
                }
            }
            return userList;
        }
        public string UpdateDetails(User userobj)
        {
            try
            {
                var encryptpassword = Base64Encode(userobj.Password);

                connection();
                SqlCommand cmd = new SqlCommand("AddOrUpdateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", userobj.Id);
                cmd.Parameters.AddWithValue("@Username", userobj.UserName);
                cmd.Parameters.AddWithValue("@Email", userobj.Email);
                cmd.Parameters.AddWithValue("@Password", encryptpassword);
                cmd.Parameters.AddWithValue("@Dob", "null");
                cmd.Parameters.AddWithValue("@CreatedBy", userobj.CreatedBy);
                cmd.Parameters.AddWithValue("@CreatedDate", userobj.CreatedDate);
                cmd.Parameters.AddWithValue("@ModifiedBy", userobj.ModifiedBy);
                cmd.Parameters.AddWithValue("@ModifiedDate", userobj.ModifiedDate);
                cmd.Parameters.AddWithValue("@Reqtype", userobj.Reqtype);
                cmd.Parameters.AddWithValue("@IsAdmin", userobj.IsAdmin);



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
                SqlCommand cmd = new SqlCommand("DeleteUser", con);
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

        public Login IsUserExist(string email)
        {
            Login user = new Login();
            bool IsUserExist = false;
            connection();
            SqlCommand cmd = new SqlCommand("IsUserExist", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

                if (dt.Rows.Count > 0)
                {
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Role = Convert.ToString(dt.Rows[0]["Role"]);
                IsUserExist = true;
                }
                else
                {
                
                IsUserExist = false;
                }
            if (!IsUserExist)
            {
                return null;
            }
            else
            {
                return user;
            }
        }
        public Login IsValidUser(string email, string password)
        {
            Login user = new Login();
            var encryptpassword = Base64Encode(password);
            bool IsValid = false;
            connection();
            SqlCommand cmd = new SqlCommand("IsValidUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", encryptpassword);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                
                con.Open();
            sda.Fill(dt);
            int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
               
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Password = Base64Decode(Convert.ToString(dt.Rows[0]["Password"]));
                user.Role = Convert.ToString(dt.Rows[0]["Role"]);
                dt.Clear();
                IsValid = true;
                }
            if (IsValid == true)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        public User ResetCheck(string email, string dob)
        {
            User user = new User();
            bool IsValid = false;
            connection();
            SqlCommand cmd = new SqlCommand("ResetCheck", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Dob", dob);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sda.Fill(dt);
            int i = cmd.ExecuteNonQuery();
            con.Close();
            if (dt.Rows.Count > 0)
            {
                user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                IsValid = true;
            }
            if (IsValid == true)
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        public string ResetPassword(string email, string password, int id)
        {
            try
            {
                var encryptpassword = Base64Encode(password);

                connection();
                SqlCommand cmd = new SqlCommand("ResetPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ModifiedBy", id);
                cmd.Parameters.AddWithValue("@ModifiedByDate", DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@Password", encryptpassword);
                



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

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
       
    }
}