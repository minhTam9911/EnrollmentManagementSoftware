using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class GradeTypeDto
{
	[Required]
	public int? AcademicYearId { get; set; }
	[Required]
	public int?  SubjectId { get; set; }
	[Required]
	public int? GradingMethodId { get; set; }
	[Required]
	public int? NumberOfColumn { get; set; }
	[Required]
	public int? NumberOfRequiredColumn { get; set; }
}
