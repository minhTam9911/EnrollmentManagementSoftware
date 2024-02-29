using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class ForgotPasswordDto
{
	[Required]
	[EmailAddress]
	public string? Email { get; set; }
}
