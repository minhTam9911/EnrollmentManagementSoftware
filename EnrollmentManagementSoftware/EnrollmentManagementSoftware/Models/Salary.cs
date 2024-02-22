using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class Salary
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public virtual Teacher? Teacher { get; set; }
	public decimal? RevenueAcademicYear { get; set; }
	public int? TotalStudent { get; set; }
	public decimal? Subsidize { get; set; }
	public string? Description { get; set; }
	public decimal? Wage { get; set; }
	public decimal? TotalAmount { get; set; }
	public bool IsStatus { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public virtual User? CreateBy { get; set; }
}
