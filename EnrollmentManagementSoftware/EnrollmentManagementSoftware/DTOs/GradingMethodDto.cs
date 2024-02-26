using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class GradingMethodDto
{
	[Required]
	public string? Name { get; set; }
	[Required]
	public decimal? MultiplierFactor { get; set; }
}
