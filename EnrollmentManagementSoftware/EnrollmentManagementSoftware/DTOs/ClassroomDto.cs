using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class ClassroomDto
{
	[Required]
	public string? Code { get; set; }
	[Required]
	public string? Name { get; set; }
	[Required]
	public int CourseId { get; set; }
	[Required]
	public int AcademicYearId { get; set; }
	[Required]
	public int? Capacity { get; set; }
	[Required]
	public decimal? TuitionFee { get; set; }
	public string? Description { get; set; }
	
}
