using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.Models;

public partial class User
{
	[Key]
	public Guid Id { get; set; }
	public string? Email { get; set; }
	public string? FullName { get; set; }
	public string? Password { get; set; }	
	public string? Image { get; set;}
	public virtual Role? Role { get; set; }
	public bool? IsStatus { get; set; }
	public decimal? Wage { get; set; }
	public bool? IsPaid { get; set; }
	public string? SecurityCode {  get; set; }
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
	public string? RefreshToken { get; set; }
	public DateTime? RefreshTokenExipres { get; set; }
	public string? PasswordResetToken { get; set; }
	public DateTime? ResetTokenExpires { get; set; }

}
