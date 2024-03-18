using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class TuitionPaymentDto
{
	[Required]
	public decimal? Discount { get; set; }
	public string? Description { get; set; }
	[Required]
	public string? PaymentMethod { get; set; }
	[Required]
	public decimal? Surcharge { get; set; }
	[Required]
	public Guid? StudentId { get; set; }
	[Required]
	public int? ClassroomId { get; set; }
	[Required]
	public int? TuititionTypeId { get; set; }
}
