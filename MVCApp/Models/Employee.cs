using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCApp.Models;

[Table("Employee")]
public class Employee
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Emp No")]
    [Range(1, int.MaxValue, ErrorMessage = "Emp No must be a positive number.")]
    public int EmpNo { get; set; }

    [Required, StringLength(100), Column(TypeName = "varchar(100)")]
    [Display(Name = "Emp Name")]
    public string EmpName { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Salary cannot be negative.")]
    public int Salary { get; set; }

    [Required, StringLength(100), Column(TypeName = "varchar(100)")]
    [Display(Name = "Dept Name")]
    public string DeptName { get; set; } = string.Empty;

    [Required, StringLength(100), Column(TypeName = "varchar(100)")]
    public string Designation { get; set; } = string.Empty;
}
