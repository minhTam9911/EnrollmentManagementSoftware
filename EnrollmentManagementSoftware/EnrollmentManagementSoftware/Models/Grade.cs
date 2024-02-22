using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class Grade
{

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public virtual Subject? Subject { get; set; }
	public virtual GradeType? GradeType { get; set; }	
	public virtual ICollection<Point>? Point { get; set; } = new List<Point>();
	public virtual Student? Student { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }

}

public class Point
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public decimal? Score { get; set; }
}


