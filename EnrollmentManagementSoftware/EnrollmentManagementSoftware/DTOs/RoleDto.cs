using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class RoleDto
{
	[Required]
	public string? Name { get; set; }
	[Required]
	public List<string>? PermissionsId { get; set; } = new List<string>();
}
