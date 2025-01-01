using SchoolManagement.MVC.Data;
using SchoolManagementApp.MVC.Models;

public class ClassEnrollmentVM{
    public ClassesDatum? ClassesDatum { get; set;}
    public List<StudentEnrollmentVM> Students { get; set;}=new 
    List<StudentEnrollmentVM>();
}

