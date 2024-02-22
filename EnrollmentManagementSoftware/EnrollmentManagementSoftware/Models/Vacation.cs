using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class Vacation
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public string? Name { get; set; }
	public string? Reason { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}
