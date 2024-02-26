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
	public List<Point>? Point { get; set; } = new List<Point>();
	[Required]
	public Guid StudentId { get; set; }
}
