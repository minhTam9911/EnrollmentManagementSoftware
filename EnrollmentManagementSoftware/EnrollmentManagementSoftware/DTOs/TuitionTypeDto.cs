using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class TuitionTypeDto
{
	[Required]
	public string? Name { get; set; }
}
