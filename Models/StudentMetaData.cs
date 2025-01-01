using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SchoolManagementApp.MVC.Data;
// namespace SchoolManagementApp.MVC.Models;


public class StudentMetaData{
    [StringLength(50)]
    [Display(Name="First name")]
    public string FisrtName { get; set; } = null!;
    [Display(Name="Last Name")]
    public string LastName { get; set; } = null!;
    // [Required]
    [Display(Name ="Date Of Birth")]
    public DateOnly? DateOfBirth { get; set; }   
}

[ModelMetadataType(typeof(StudentMetaData))]
public partial class Student{}