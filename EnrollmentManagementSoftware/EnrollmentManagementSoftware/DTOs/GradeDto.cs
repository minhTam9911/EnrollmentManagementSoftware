using AutoMapper.Configuration.Annotations;
using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class GradeDto
{
	[Required]
	public int? SubjectId { get; set; }
	[Required]
	public int? GradeTypeId { get; set; }
	[Required]
	[Ignore]
	public List<int>? Points { get; set; } = new List<int>();
	[Required]
	public Guid StudentId { get; set; }
}
