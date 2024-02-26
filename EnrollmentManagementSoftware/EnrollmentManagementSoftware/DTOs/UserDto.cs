using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class UserDto
{
	[Required]
	[EmailAddress]
	public string? Email { get; set; }
	[Required]
	public string? FullName { get; set; }
	[Required]
	public string? Password { get; set; }
	[Required]
	public int? RoleId { get; set; }
	[Required]
	public bool? IsStatus { get; set; }
}
