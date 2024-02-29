using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class ResetPasswordDto
{
	[Required]
	public string? Token { get; set; }
	[Required]
	public string? NewPassword { get; set; }
	[Required]
	public string? ConfirmPassword { get; set; }
}
