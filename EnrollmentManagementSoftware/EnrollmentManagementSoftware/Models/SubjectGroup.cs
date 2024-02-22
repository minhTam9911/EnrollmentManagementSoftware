using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class SubjectGroup
{

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public string? Name { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}
