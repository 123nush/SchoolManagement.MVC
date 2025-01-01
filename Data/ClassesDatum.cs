using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.MVC.Data;

public partial class ClassesDatum
{
    public int Id { get; set; }
    [Display(Name = "Lecturer")]
    public int? LecturerId { get; set; }
    [Display(Name = "Course")]
    public int? CourseId { get; set; }

    public TimeOnly? Time { get; set; }

    public virtual Course? Course { get; set; }

    public virtual ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    // one to many relationship between classes and enrollments
    public virtual Lecturer? Lecturer { get; set; }
}
