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
                            Deleted = Convert.ToBoolean(dr["Deleted"]),
                            Created_by = Convert.ToInt32(dr["Created_by"]),
                            Created_date = Convert.ToString(dr["Created_date"]),
                            Modified_by = Convert.ToInt32(dr["Modified_by"]),
                            Modified_date = Convert.ToString(dr["Modified_date"]),
                        });
                }
                foreach (var n in datalist)
                {

                    var creteUser = GetByIdOfUser(n.Created_by);
                    n.Created_Name = creteUser.Email;
                    var Updateuser = GetByIdOfUser(n.Modified_by);
                    n.Modified_Name = Updateuser.Email;

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
            cmd.Parameters.AddWithValue("@Created_by", obj.Created_by);
            cmd.Parameters.AddWithValue("@Created_date", obj.Created_date);
            cmd.Parameters.AddWithValue("@Modified_by", obj.Modified_by);
            cmd.Parameters.AddWithValue("@Modified_date", obj.Modified_date);

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

        public bool DeleteUser(int id)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("DeleteUser", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Id", id);

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
                obj.Created_by = Convert.ToInt32(dt.Rows[0]["Created_by"]);
                obj.Created_date = Convert.ToString(dt.Rows[0]["Created_date"]);
                obj.Modified_by = Convert.ToInt32(dt.Rows[0]["Modified_by"]);
                obj.Modified_date = Convert.ToString(dt.Rows[0]["Modified_date"]);           
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
                            Created_by = Convert.ToInt32(dr["Created_by"]),
                            Created_date = Convert.ToString(dr["Created_date"]),
                            Modified_by = Convert.ToInt32(dr["Modified_by"]),
                            Modified_date = Convert.ToString(dr["Modified_date"]),

                        });
                }
                foreach (var n in PatientList)
                {

                    var creteUser = GetByIdOfUser(n.Created_by);
                    n.Created_Name = creteUser.Email;
                    var Updateuser = GetByIdOfUser(n.Modified_by);
                    n.Modified_Name = Updateuser.Email;

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

        public int GetFullCountOfUsers(string searchValue)
        {
            int count = 0;
            Connection();
            SqlCommand cmd = new SqlCommand("GetFullCountOfUser", con)
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

            cmd.Parameters.AddWithValue("@Email", Username);
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

            cmd.Parameters.AddWithValue("@Email", Username);
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
            cmd.Parameters.AddWithValue("@Email", Username);
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
                    obj.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    obj.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                    obj.PhoneNumber = Convert.ToString(dt.Rows[0]["PhoneNumber"]);
                    obj.Password = Convert.ToString(dt.Rows[0]["Password"]);
                    obj.IsAdmin = Convert.ToInt32(dt.Rows[0]["IsAdmin"]);
                    obj.Created_by = Convert.ToInt32(dt.Rows[0]["Created_by"]);
                    obj.Created_date = Convert.ToString(dt.Rows[0]["Created_date"]);
                    obj.Modified_by = Convert.ToInt32(dt.Rows[0]["Modified_by"]);
                    obj.Modified_date = Convert.ToString(dt.Rows[0]["Modified_date"]);
                }
                
            }
            return obj;
        }

        public Registration GetByUsernameReset(string Username, string phone)
        {
            Connection();
            Registration obj = new Registration();
            SqlCommand cmd = new SqlCommand("GetByUserNameReset", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Email", Username);
            cmd.Parameters.AddWithValue("@phone", phone);
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
                    obj.Email = Convert.ToString(dt.Rows[0]["Email"]);
                    obj.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                    obj.PhoneNumber = Convert.ToString(dt.Rows[0]["PhoneNumber"]);
                    obj.Password = Convert.ToString(dt.Rows[0]["Password"]);
                    obj.IsAdmin = Convert.ToInt32(dt.Rows[0]["IsAdmin"]);
                }
            }
            return obj;
        }

        public bool AddUser(Registration rmodel)
        {
            try
            {
                Connection();
                SqlCommand cmd = new SqlCommand("AddUsers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
             
                cmd.Parameters.AddWithValue("@Name", rmodel.Name);
                cmd.Parameters.AddWithValue("@Email", rmodel.Email);
                cmd.Parameters.AddWithValue("@Designation", rmodel.Designation);
                cmd.Parameters.AddWithValue("@PhoneNumber", rmodel.PhoneNumber);
                cmd.Parameters.AddWithValue("@Password", rmodel.Password);        
                cmd.Parameters.AddWithValue("@IsAdmin", 0);
                cmd.Parameters.AddWithValue("@Created_by", rmodel.Created_by);
                cmd.Parameters.AddWithValue("@Created_date", rmodel.Created_date);
                cmd.Parameters.AddWithValue("@Modified_by", rmodel.Modified_by);
                cmd.Parameters.AddWithValue("@Modified_date", rmodel.Modified_date);

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

        public List<Registration> GetReg(int offsetValue, int PagingSize, string search)
        {
            Connection();
            List<Registration> reglist = new List<Registration>();

            SqlCommand cmd = new SqlCommand("GetRegisters", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@offsetValue", offsetValue);
            cmd.Parameters.AddWithValue("@PagingSize", PagingSize);
            cmd.Parameters.AddWithValue("@search", search);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                reglist.Add(
                    new Registration
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                        Email = Convert.ToString(dr["Email"]),
                        Designation = Convert.ToString(dr["Designation"]),
                        PhoneNumber = Convert.ToString(dr["PhoneNumber"]),
                        IsAdmin = Convert.ToInt32(dr["IsAdmin"]),
                        Created_by = Convert.ToInt32(dr["Created_by"]),
                        Created_date = Convert.ToString(dr["Created_date"]),
                        Modified_by = Convert.ToInt32(dr["Modified_by"]),
                        Modified_date = Convert.ToString(dr["Modified_date"]),
                    });
            }
            foreach (var n in reglist)
            {
                
                var creteUser = GetByIdOfAdmin(n.Created_by);
                n.Created_Name = creteUser.Email;
                var Updateuser = GetByIdOfAdmin(n.Modified_by);
                n.Modified_Name = Updateuser.Email;
               
            }
            
            return reglist;
        }

        public List<Registration> GetUserInfo(int pageIndex, int pageSize, string searchValue)
        {

            Connection();
            List<Registration> datalists = new List<Registration>();

            SqlCommand cmd = new SqlCommand("GetRegisters", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@PageSize", pageSize);
            cmd.Parameters.AddWithValue("@SearchValue", searchValue);
           

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            if (con.State == ConnectionState.Open)
            {

                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    datalists.Add(
                        new Registration
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            Email = Convert.ToString(dr["Email"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            PhoneNumber = Convert.ToString(dr["PhoneNumber"]),
                            IsAdmin = Convert.ToInt32(dr["IsAdmin"]),
                            
                        });
                }
            }
            return datalists;
        }
       
        public Registration GetByIdOfUser(int id)
        {
            Connection();
            Registration obj = new Registration();
            SqlCommand cmd = new SqlCommand("GetByIdOfAdmin", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                sd.Fill(dt);
               
                obj.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                obj.Name = Convert.ToString(dt.Rows[0]["Name"]);
                obj.Email = Convert.ToString(dt.Rows[0]["Email"]);              
                obj.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                obj.PhoneNumber = Convert.ToString(dt.Rows[0]["PhoneNumber"]);
                obj.Password = Convert.ToString(dt.Rows[0]["Password"]);
                obj.IsAdmin = Convert.ToInt32(dt.Rows[0]["IsAdmin"]);
                obj.Created_by = Convert.ToInt32(dt.Rows[0]["Created_by"]);
                obj.Created_date = Convert.ToString(dt.Rows[0]["Created_date"]);
               
            }
            con.Close();
            return obj;
        }

        public Registration GetByIdOfAdmin(int id)
        {
            Connection();
            Registration obj = new Registration();
            SqlCommand cmd = new SqlCommand("GetByIdOfAdmin", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            if (con.State == ConnectionState.Open)
            {
                sd.Fill(dt);

                obj.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                obj.Name = Convert.ToString(dt.Rows[0]["Name"]);
                obj.Email = Convert.ToString(dt.Rows[0]["Email"]);
                obj.Designation = Convert.ToString(dt.Rows[0]["Designation"]);
                obj.PhoneNumber = Convert.ToString(dt.Rows[0]["PhoneNumber"]);
                obj.Password = Convert.ToString(dt.Rows[0]["Password"]);
                obj.IsAdmin = Convert.ToInt32(dt.Rows[0]["IsAdmin"]);
                obj.Created_by = Convert.ToInt32(dt.Rows[0]["Created_by"]);
                obj.Created_date = Convert.ToString(dt.Rows[0]["Created_date"]);
                obj.Modified_by= Convert.ToInt32(dt.Rows[0]["Modified_by"]);
                obj.Modified_date = Convert.ToString(dt.Rows[0]["Modified_date"]);

            }
            con.Close();
            return obj;
        }

        public bool UpdateUserDetails(Registration obj)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("UpdateUserDetails", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Id", obj.Id);
            cmd.Parameters.AddWithValue("@Name", obj.Name);
            cmd.Parameters.AddWithValue("@Email", obj.Email);
            cmd.Parameters.AddWithValue("@Designation", obj.Designation);
            cmd.Parameters.AddWithValue("@PhoneNumber", obj.PhoneNumber);
            cmd.Parameters.AddWithValue("@Password", obj.Password);
            cmd.Parameters.AddWithValue("@IsAdmin", 0);
            cmd.Parameters.AddWithValue("@Created_by", obj.Created_by);
            cmd.Parameters.AddWithValue("@Created_date", obj.Created_date);
            cmd.Parameters.AddWithValue("@Modified_by", obj.Modified_by);
            cmd.Parameters.AddWithValue("@Modified_date", obj.Modified_date);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        public List<Registration> GetAllUSers()
        {
            Connection();
            List<Registration> registredList = new List<Registration>();

            SqlCommand cmd = new SqlCommand("GetAllUsers", con);
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
                    registredList.Add(
                        new Registration
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            Email = Convert.ToString(dr["Email"]),
                            Designation = Convert.ToString(dr["Designation"]),
                            PhoneNumber = Convert.ToString(dr["PhoneNumber"]),
                            IsAdmin = Convert.ToInt32(dr["IsAdmin"]),
                        });
                }
            }
            catch (Exception)
            {
                throw;
            }
            return registredList;
        }
    }
}
