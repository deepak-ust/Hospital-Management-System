using Hospital_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Hospital_Management_System.DAL
{
    public class DBHelper
    {
        private SqlConnection con;
        private void Connection()
        {
            string constring = "Data Source=DESKTOP-BC5TS1F\\MSSQLSERVER2019;Initial Catalog=Patient;Integrated Security=True";
            con = new SqlConnection(constring);
        }

        //View patient details
        public List<Patient> GetData(int pageIndex, int pageSize, string searchValue, string column, string order)
        {

            Connection();
            List<Patient> datalist = new List<Patient>();

            SqlCommand cmd = new SqlCommand("GetData", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SearchValue", searchValue);
            cmd.Parameters.AddWithValue("@ColumnName", column);
            cmd.Parameters.AddWithValue("@Order", order);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            if (con.State == ConnectionState.Open)
            {

                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    datalist.Add(
                        new Patient
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            Age = Convert.ToInt32(dr["Age"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            Date = Convert.ToDateTime(dr["dateTime"]),
                            InPatient = Convert.ToBoolean(dr["InPatient"]),
                            Deleted = Convert.ToBoolean(dr["Deleted"])
                        });
                }
            }
            return datalist;
        }

        //Update or add patient details
        public bool UpdateDetails(Patient obj)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("UpdateData", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatId", obj.Id);
            cmd.Parameters.AddWithValue("@Name", obj.Name);
            cmd.Parameters.AddWithValue("@LastName", obj.LastName);
            cmd.Parameters.AddWithValue("@Age", obj.Age);
            cmd.Parameters.AddWithValue("@Gender", obj.Gender);
            cmd.Parameters.AddWithValue("@Date", obj.Date);
            cmd.Parameters.AddWithValue("@InPatient", obj.InPatient);
            cmd.Parameters.AddWithValue("@Deleted", obj.Deleted);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //Delete patient details
        public bool DeleteData(int id)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("DeleteData", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatId", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //Get patient by id
        public Patient GetById(int id)
        {
            Connection();
            Patient obj = new Patient();
            SqlCommand cmd = new SqlCommand("GetById", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@id", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                sd.Fill(dt);
                con.Close();
                obj.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                obj.Name = Convert.ToString(dt.Rows[0]["Name"]);
                obj.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                obj.Age = Convert.ToInt32(dt.Rows[0]["Age"]);
                obj.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                obj.Date = Convert.ToDateTime(dt.Rows[0]["dateTime"]);
                obj.InPatient = Convert.ToBoolean(dt.Rows[0]["InPatient"]);
                obj.Deleted = Convert.ToBoolean(dt.Rows[0]["Deleted"]);
            }
            return obj;
        }

        public List<Patient> GetAll()
        {
            Connection();
            List<Patient> PatientList = new List<Patient>();

            SqlCommand cmd = new SqlCommand("GetAll", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    PatientList.Add(
                        new Patient
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            LastName = Convert.ToString(dr["LastName"]),
                            Age= Convert.ToInt32(dr["Age"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            Date = Convert.ToDateTime(dr["dateTime"]),
                           InPatient= Convert.ToBoolean(dr["InPatient"]),
                            

                        });
                }
            }
            catch (Exception )
            {
                throw;
            }
            return PatientList;
        }

        public int GetFullCount(string searchValue)
        {
            int count = 0;
            Connection();
            SqlCommand cmd = new SqlCommand("GetFullCount", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@SearchValue", searchValue);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                sd.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    count = Convert.ToInt32(row["Column1"]);
                }
            }
            return count;
        }


        public int GetRegister(string Username, string Password)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("User_Login", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            SqlParameter objLogin = new SqlParameter();
            objLogin.ParameterName = "@IsValid";
            objLogin.Direction = ParameterDirection.Output;
            objLogin.SqlDbType = SqlDbType.Bit;
            cmd.Parameters.Add(objLogin);
                
            con.Open();
            int i = cmd.ExecuteNonQuery();
            int result = Convert.ToInt32(objLogin.Value);
            con.Close();
            return result;
            
            //if (i >= 1)
            //    return true;
            //else
            //    return false;
        }
        public int IsAdminCheck(string Username, string Password)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("IsAdminUser", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Username", Username);
            cmd.Parameters.AddWithValue("@Password", Password);
            SqlParameter objLogin = new SqlParameter();
            objLogin.ParameterName = "@IsValid";
            objLogin.Direction = ParameterDirection.Output;
            objLogin.SqlDbType = SqlDbType.Bit;
            cmd.Parameters.Add(objLogin);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            int result = Convert.ToInt32(objLogin.Value);
            con.Close();
            return result;
        }

        public Registration GetByUsername(string Username)
        {
            Connection();
            Registration obj = new Registration();
            SqlCommand cmd = new SqlCommand("GetByUserName", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@userName", Username);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                sd.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    obj.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                    obj.Name = Convert.ToString(dt.Rows[0]["Name"]);
                    obj.UserName = Convert.ToString(dt.Rows[0]["UserName"]);
                    obj.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                    obj.PhoneNumber = Convert.ToString(dt.Rows[0]["PhoneNumber"]);
                    obj.Password = Convert.ToString(dt.Rows[0]["Password"]);
                    obj.IsAdmin = Convert.ToInt32(dt.Rows[0]["IsAdmin"]);
                }
                
            }
            return obj;
        }

        public bool AddUser(Registration smodel)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("RegUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StdId", smodel.Id);
                cmd.Parameters.AddWithValue("@Name", smodel.Name);
                cmd.Parameters.AddWithValue("@UserName", smodel.UserName);
                cmd.Parameters.AddWithValue("@Designation", smodel.Designation);
                cmd.Parameters.AddWithValue("@PhoneNumber", smodel.PhoneNumber);
                cmd.Parameters.AddWithValue("@Password", smodel.Password);        
                cmd.Parameters.AddWithValue("@IsAdmin", 0);

                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();

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
    }
}
