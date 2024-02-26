using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class SalaryDto
{
	[Required]
	public  Guid? TeacherId { get; set; }
	[Required]
	public decimal? RevenueAcademicYear { get; set; }
	[Required]
	public int? TotalStudent { get; set; }
	[Required]
	public decimal? Subsidize { get; set; }
	public string? Description { get; set; }
	[Required]
	public decimal? Wage { get; set; }
	[Required]
	public decimal? TotalAmount { get; set; }
	[Required]
	public bool IsStatus { get; set; }
}
