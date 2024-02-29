using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class VerifySecurityCodeDto
{
	[Required]
	public string? Token { get; set; }
	[Required]
	[MaxLength(6)]
	[MinLength(6)]
	public string? Code { get; set; }
}
