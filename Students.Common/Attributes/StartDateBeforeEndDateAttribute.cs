using Students.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Students.Common.Attributes
{
    public class StartDateBeforeEndDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var subject = (Subject)validationContext.ObjectInstance;

            if (subject.StartDate > subject.EndDate)
            {
                return new ValidationResult("Start date must be before end date.");
            }

            return ValidationResult.Success;
        }
    }
}
