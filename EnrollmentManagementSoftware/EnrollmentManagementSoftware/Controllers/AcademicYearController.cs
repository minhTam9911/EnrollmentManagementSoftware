using EnrollmentManagementSoftware.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AcademicYearController : ControllerBase
{
	[HttpPost]
	public IActionResult Insert([FromBody] AcademicYearDto academicYearDto)
	{
		DateOnly dateOnly = (DateOnly)academicYearDto.StartDate;
		DateTime date =dateOnly.ToDateTime(TimeOnly.MinValue);
		return Ok(academicYearDto.EndDate);
	}

}
