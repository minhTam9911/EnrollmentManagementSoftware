using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class CourseDto
{
	[Required]
	public string? Name { get; set; }
}
