using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Students.Common.Attributes
{
    public class SubjectCantStartWithNumbersOrLowercaseAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is string str)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return new ValidationResult("Subject can't be empty. ");
                }
                else if (char.IsLower(str[0]))
                {
                    return new ValidationResult("Subject can't start with lowercase. ");
                }
                else if (Regex.IsMatch(str, @"\d"))
                {
                    return new ValidationResult("Subject name can't contain numbers. ");
                }
            }

            return ValidationResult.Success;
        }
    }
}
