using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public class Invoice
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public decimal? Discount { get; set; }
	public string? Description { get; set; }
	public string? PaymentMethod { get; set; }
	public decimal? Surcharge { get; set; }
	public decimal? Amount { get; set; }
	public virtual Student? Student { get; set; }
	public virtual Classroom? Classroom { get; set; }
	public virtual TuitionType? TuititionType { get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public virtual User? CreateBy { get; set; }
}
