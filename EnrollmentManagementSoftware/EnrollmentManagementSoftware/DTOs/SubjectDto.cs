using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class SubjectDto
{
	[Required]
	public string? Code { get; set; }
	[Required]
	public string? Name { get; set; }
	[Required]
	public int? SubjectGroupId { get; set; }
	[Required]
	public int? CourseId { get; set; }
}
