using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class Salary
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public virtual User? Employee { get; set; }
	public virtual Course? Course { get; set; }
	public decimal? WagePercent { get; set; }
	public decimal? Subsidize { get; set; }
	public string? SubsidizeName { get; set; }
	public string? Description { get; set; }
	public decimal? TotalAmount { get; set; }
	public bool IsStatus { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public virtual User? CreateBy { get; set; }
}
