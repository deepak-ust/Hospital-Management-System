using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HospitalManagementLibrary
{
    public class Patient
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [RegularExpression("[A-Za-z ]{1,30}", ErrorMessage = "Give a proper name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1, 120, ErrorMessage = "Not valid")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        [GenderValidation(ErrorMessage = "Enter valid gender")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Required")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime dateTime { get; set; }

        [Required(ErrorMessage = "Status is required")]
        public bool InPatient { get; set; }
        public bool Deleted { get; set; }
    }
}
