using AutoMapper.Configuration.Annotations;
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
	[RegularExpression(pattern: "^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$",
					ErrorMessage = "Password must be at least 8 characters long. Must include uppercase letters," +
									" lowercase letters, numbers and special characters")]
	public string? Password { get; set; }
	[Required]
	public int? RoleId { get; set; }
	[Required]
	public bool? IsStatus { get; set; }
	[Ignore]
	public IFormFile? Image { get; set; }
	[Required]
	public decimal Wage { get; set; }
}
