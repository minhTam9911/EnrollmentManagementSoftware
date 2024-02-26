using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class PermissionDto
{
	[Required]
	public string? Name { get; set; }
}
