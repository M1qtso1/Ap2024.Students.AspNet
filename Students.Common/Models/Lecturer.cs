using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Students.Common.Attributes;

namespace Students.Common.Models;
public class Lecturer
{
    public int Id { get; set; }
    [NameShouldNotStartWithLowercase]
    public string Name { get; set; } = string.Empty;
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public int Age { get; set; }
    [NotMapped]
    public ICollection<Subject>? AvailableSubjects { get; set; } = default!;
}