using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCApp.Models;

[Table("Department")]
public class Department
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    [Display(Name = "Dept No")]
    [Range(1, int.MaxValue, ErrorMessage = "Dept No must be a positive number.")]
    public int DeptNo { get; set; }

    [Required, StringLength(100), Column(TypeName = "varchar(100)")]
    [Display(Name = "Dept Name")]
    public string DeptName { get; set; } = string.Empty;

    [Required, StringLength(100), Column(TypeName = "varchar(100)")]
    public string Location { get; set; } = string.Empty;
}
