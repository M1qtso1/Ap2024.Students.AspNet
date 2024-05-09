using Students.Common.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Students.Common.Attributes
{
    public class ValidateDateNotInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dateValue = (DateTime)value;
            if (dateValue.Date > DateTime.Now.Date)
            {
                return new ValidationResult("Dates can't be in the future, we're not in the movie!");
            }
            
            return ValidationResult.Success;
        }
    }
}
