using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class GradeType
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public virtual AcademicYear? AcademicYear { get; set; }
	public virtual Subject? Subject { get; set; }
	public virtual GradingMethod? GradingMethod { get; set; }
	public int? NumberOfColumn { get; set; }
	public int? NumberOfRequiredColumn { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public virtual User? CreateBy { get; set; }
}