using System;
using System.Collections.Generic;

namespace SchoolManagement.MVC.Data;

public partial class Student
{
    public int Id { get; set; }

    public string FisrtName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }
}
