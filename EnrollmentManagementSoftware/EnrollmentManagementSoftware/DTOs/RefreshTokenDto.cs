using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class RefreshTokenDto
{
	[Required]
	public string? AccessToken { get; set; }
	[Required]
	public string? RefreshToken { get; set; }
	public DateTime? ExipresRefreshToken { get; set; }
}
