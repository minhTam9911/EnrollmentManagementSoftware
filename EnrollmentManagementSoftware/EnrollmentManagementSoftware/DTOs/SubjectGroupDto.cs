using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class SubjectGroupDto
{
	[Required]
	public string? Name { get; set; }
}
