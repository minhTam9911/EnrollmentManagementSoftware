using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.DTOs;

public class SalaryDto
{
	[Required]
	public  Guid? EmployeeId { get; set; }
	[Required]
	public decimal? WagePercent { get; set; }
	public decimal? Subsidize { get; set; }
	public string? SubsidizeName { get; set; }
	public string? Description { get; set; }
	public int? CourseId { get; set; }
}
