using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class AcademicYearDto
{
	[Required]
	public string? Code { get; set; }
	[Required]
	public string? Name { get; set; }
	[Required]
	public DateOnly StartDate { get; set; }
	[Required]
	public DateOnly EndDate { get; set; }
}
