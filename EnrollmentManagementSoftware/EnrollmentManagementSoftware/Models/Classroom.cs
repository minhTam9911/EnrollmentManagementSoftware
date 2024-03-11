using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class Classroom
{

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public string? Code { get;set; }
	public string? Name { get; set; }	
	public virtual Course? Course { get; set; }
	public virtual AcademicYear? AcademicYear { get; set; }
	public int? Capacity { get; set; }
	public decimal? TuitionFee { get; set; }	
	public string? Description { get; set; }
	public string? Image { get; set; }
	public bool Status {  get; set; }
	public virtual ICollection<Schedule>? Schedules { get; set; } = new List<Schedule>();
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public virtual User? CreateBy { get; set; }
}
