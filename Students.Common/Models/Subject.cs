using Students.Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Students.Common.Models;

public class Subject
{
    public int Id { get; set; }
    [SubjectCantStartWithNumbersOrLowercase]
    public string Name { get; set; } = string.Empty;
    public int Credits { get; set; }

    public List<Student> Students { get; set; } = new List<Student>();

    public ICollection<StudentSubject> StudentSubjects { get; set; } = new List<StudentSubject>();
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    [Required(ErrorMessage = "Start date is required")]
    [ValidateDateNotInFuture(ErrorMessage = "Start date cannot be in the future")]
    [StartDateBeforeEndDate]
    public DateTime StartDate { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    [Required(ErrorMessage = "End date is required")]
    [ValidateDateNotInFuture(ErrorMessage = "End date cannot be in the future")]
    [StartDateBeforeEndDate]
    public DateTime EndDate { get; set; }
public Subject()
    {
    }

    public Subject(string name, int credits, DateTime startDate, DateTime endDate)
    {
        Name = name;
        Credits = credits;
        StartDate = startDate;
        EndDate = endDate;
    }
}
