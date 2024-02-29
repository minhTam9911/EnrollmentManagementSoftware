using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class ChangePasswordDto
{
	[Required]
	public string? CurrentPassword { get; set; }
	[Required]
	public string? NewPassword { get; set; }
	[Required]
	public string? ConfirmPassword { get; set; }
}
