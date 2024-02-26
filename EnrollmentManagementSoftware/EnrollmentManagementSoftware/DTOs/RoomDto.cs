using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class RoomDto
{
	[Required]
	public string? Name { get; set; }
	[Required]
	public int? Capacity { get; set; }
	[Required]
	public string? Facilities { get; set; }
}
