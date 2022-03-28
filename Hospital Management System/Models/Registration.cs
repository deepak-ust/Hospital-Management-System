﻿using HospitalManagementLibrary.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hospital_Management_System.Models
{
    public class Registration
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("[A-Za-z ]{1,30}", ErrorMessage = "Give a proper name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "User name is required")]    
        public string UserName { get; set; }

        [Required(ErrorMessage = "Designation is required")]      
        public string Designation { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        public int IsAdmin { get; set; }

        public string Created_by { get; set; }

        public string Created_date { get; set; }

        public string Modified_by { get; set; }

        public string Modified_date { get; set; }

        public Operations ActionType { get; set; }
    }
}