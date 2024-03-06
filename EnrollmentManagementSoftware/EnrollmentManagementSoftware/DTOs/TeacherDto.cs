using AutoMapper.Configuration.Annotations;
using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class TeacherDto
{
	[Required]
	public string? FirstName { get; set; }
	[Required]
	public string? LastName { get; set; }
	[Required]
	public string? TaxCode { get; set; }
	[Required]
	public string? Address { get; set; }
	[Required]
	public string? Password { get; set; }
	[Required]
	[Phone]
	public string? PhoneNumber { get; set; }
	[Required]
	[EmailAddress]
	public string? Email { get; set; }
	[Required]
	public string? Gender { get; set; }
	[Required]
	public DateOnly DayOfBirth { get; set; }
	[Required]
	public decimal? Wage { get; set; }
	[Required]
	public int? MajorSubjectId { get; set; }
	[Required]
	public int? MinorSubjectId { get; set; }
	[Ignore]
	public IFormFile? Image { get; set; }
}
