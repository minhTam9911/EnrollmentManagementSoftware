using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class RoleDto
{
	[Required]
	public string? Name { get; set; }
	
	public List<int>? PermissionsId { get; set; } = new List<int>();
}
