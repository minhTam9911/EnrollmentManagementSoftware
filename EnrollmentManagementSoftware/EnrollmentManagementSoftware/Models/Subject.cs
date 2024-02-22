using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class Subject
{

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public string? Code { get; set; }
	public string? Name { get; set; }
	public virtual SubjectGroup? SubjectGroup { get; set; }	
	public virtual Course? Course { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}
