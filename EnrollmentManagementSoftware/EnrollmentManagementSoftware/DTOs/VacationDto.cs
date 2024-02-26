using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class VacationDto
{
	[Required]
	public string? Name { get; set; }
	[Required]
	public string? Reason { get; set; }
	[Required]
	public DateOnly StartDate { get; set; }
	[Required]
	public DateOnly EndDate { get; set; }
}
