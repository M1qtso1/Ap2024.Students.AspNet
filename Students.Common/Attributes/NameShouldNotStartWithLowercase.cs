using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Students.Common.Attributes
{
    public class NameShouldNotStartWithLowercaseAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string fullName)
            {
                if (string.IsNullOrEmpty(fullName))
                {
                    return new ValidationResult("Name can't be empty. ");
                }

                if (char.IsLower(fullName[0]))
                {
                    return new ValidationResult("Name should not start with lowercase.");
                }

                if (Regex.IsMatch(fullName, @"^[A-Z][a-zA-Z]*\s[A-Z][a-zA-Z]*$"))
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult("Name should not contain special symbols or more then one space.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
    
