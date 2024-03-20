using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class PermissionDto
{
	[Required]
	public string? Name { get; set; }
	[Required]
	public string? Descritpion { get; set; }
}
