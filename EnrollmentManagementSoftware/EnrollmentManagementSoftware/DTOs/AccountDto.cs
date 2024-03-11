using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class AccountDto
{
	[Required]
	[EmailAddress]
	public string? Username { get; set; }
	[Required]
	public string? Password { get; set; }
}
