using System;
using System.ComponentModel.DataAnnotations;

namespace HospitalManagementLibrary.Validations
{
    public class GenderValidation:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (Convert.ToString(value) == "Male" || Convert.ToString(value) == "Female" || Convert.ToString(value) == "Other" || Convert.ToString(value) == "male" || Convert.ToString(value) == "female" || Convert.ToString(value) == "other")
                return ValidationResult.Success;
            else
                return new ValidationResult(ErrorMessage);
        }
    }
}
