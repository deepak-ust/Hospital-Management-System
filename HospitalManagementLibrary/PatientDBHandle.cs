using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace HospitalManagementLibrary
{
   public class PatientDBHandle
    {
        private SqlConnection con;
        private void Connection()
        {
            string constring = "Data Source=LAPTOP-5IQ1TLRU;Initial Catalog=Patients;Integrated Security=True";
            con = new SqlConnection(constring);
        }

        

        //View patient details
        public List<Patient> GetPatient(int pageIndex,int pageSize, string searchValue)
        {

            Connection();
            List<Patient> patientlist = new List<Patient>();

            SqlCommand cmd = new SqlCommand("GetPatients", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@pageSize", pageSize);
            cmd.Parameters.AddWithValue("@searchValue", searchValue);

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            if (con.State == ConnectionState.Open)
            {

                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    patientlist.Add(
                        new Patient
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            Age = Convert.ToInt32(dr["Age"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            dateTime = Convert.ToDateTime(dr["dateTime"]),
                            InPatient = Convert.ToBoolean(dr["InPatient"]),
                            Deleted = Convert.ToBoolean(dr["Deleted"])
                        });
                }
            }
            return patientlist;
        }

        //Update or add patient details
        public bool UpdateDetails(Patient pmodel)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("UpdatePatientDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PatId", pmodel.Id);
            cmd.Parameters.AddWithValue("@Name", pmodel.Name);
            cmd.Parameters.AddWithValue("@Age", pmodel.Age);
            cmd.Parameters.AddWithValue("@Gender", pmodel.Gender);
            cmd.Parameters.AddWithValue("@dateTime", pmodel.dateTime);
            cmd.Parameters.AddWithValue("@InPatient", pmodel.InPatient);
            cmd.Parameters.AddWithValue("@Deleted", pmodel.Deleted);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }

        //Delete patient details
        public bool DeletePatient(int id)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("DeletePatient", con);
            cmd.CommandType = CommandType.StoredProcedure;

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
            Patient pmodel = new Patient();

            SqlCommand cmd = new SqlCommand("GetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@id", id);
           

            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            if (con.State == ConnectionState.Open)
            {

                sd.Fill(dt);
                con.Close();

                pmodel.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                pmodel.Name = Convert.ToString(dt.Rows[0]["Name"]);
                pmodel.Age = Convert.ToInt32(dt.Rows[0]["Age"]);
                pmodel.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                pmodel.dateTime = Convert.ToDateTime(dt.Rows[0]["dateTime"]);
                pmodel.InPatient = Convert.ToBoolean(dt.Rows[0]["InPatient"]);
                pmodel.Deleted = Convert.ToBoolean(dt.Rows[0]["Deleted"]);
            }

            return pmodel;
        }

    }
}
