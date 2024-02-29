using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class Course
{

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public string? Name { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public virtual ICollection<Classroom>? Classrooms { get; set; } = new List<Classroom>();
	public virtual ICollection<Subject>? Subjects { get; set; } = new List<Subject>();
	public virtual User? CreateBy { get; set; }
}
