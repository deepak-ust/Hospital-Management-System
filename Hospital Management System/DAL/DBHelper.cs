﻿using Hospital_Management_System.Models;
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
            string constring = "Data Source=LAPTOP-5IQ1TLRU;Initial Catalog=Patients;Integrated Security=True";
            con = new SqlConnection(constring);
        }

        //View patient details
        public List<Patient> GetPatient(int pageIndex, int pageSize, string searchValue)
        {

            Connection();
            List<Patient> patientlist = new List<Patient>();

            SqlCommand cmd = new SqlCommand("GetPatients", con)
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
                    patientlist.Add(
                        new Patient
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            Age = Convert.ToInt32(dr["Age"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            Date = Convert.ToDateTime(dr["dateTime"]),
                            InPatient = Convert.ToBoolean(dr["InPatient"]),
                            Deleted = Convert.ToBoolean(dr["Deleted"])
                        });
                }
            }
            return patientlist;
        }

        //Update or add patient details
        public bool UpdateDetails(Patient obj)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("UpdatePatientDetails", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@PatId", obj.Id);
            cmd.Parameters.AddWithValue("@Name", obj.Name);
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
        public bool DeletePatient(int id)
        {
            Connection();
            SqlCommand cmd = new SqlCommand("DeletePatient", con)
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
            Patient pmodel = new Patient();

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

                pmodel.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                pmodel.Name = Convert.ToString(dt.Rows[0]["Name"]);
                pmodel.Age = Convert.ToInt32(dt.Rows[0]["Age"]);
                pmodel.Gender = Convert.ToString(dt.Rows[0]["Gender"]);
                pmodel.Date = Convert.ToDateTime(dt.Rows[0]["dateTime"]);
                pmodel.InPatient = Convert.ToBoolean(dt.Rows[0]["InPatient"]);
                pmodel.Deleted = Convert.ToBoolean(dt.Rows[0]["Deleted"]);
            }

            return pmodel;
        }

    }
}