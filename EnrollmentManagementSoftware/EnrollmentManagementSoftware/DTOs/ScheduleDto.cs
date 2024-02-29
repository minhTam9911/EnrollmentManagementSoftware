using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class ScheduleDto
{
	[Required]
	public int? ClassroomId { get; set; }
	[Required]
	public int? Subject { get; set; }
	[Required]
	public TimeOnly StartTime { get; set; }
	[Required]
	public TimeOnly EndTime { get; set; }
	[Required]
	public DateOnly StartDate { get; set; }
	[Required]
	public DateOnly EndDate { get; set; }
	[Required]
	public List<DateTime> Days { get; set; } = new List<DateTime>();
	[Required]
	public int? Room { get; set; }
	[Required]
	public Guid Teacher { get; set; }
}
