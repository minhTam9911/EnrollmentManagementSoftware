using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
	private readonly IStudentService studentService;
	public StudentController(IStudentService studentService)
	{
		this.studentService = studentService;
	}
	[HttpPost]
	public async Task<IActionResult> Insert([FromForm] StudentDto studentDto)
	{
		/*if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		else
		{
			if (await studentService.Insert(studentDto))
			{
				return Ok();
			}
			else
			{
				return BadRequest();
			}
		}*/
		return Ok(studentDto);
		
	}
	[HttpGet]
	[Consumes("application/json")]
	[Produces("application/json")]
	public async Task<IActionResult> GetList([FromQuery(Name = "page")] int page,[FromQuery(Name = "keyword")] string? keyword)
	{
		if (page <= 0) 	page = 1;
		
		if ((await studentService.GetListAsync(page,keyword)).status)
		{
			return Ok(await studentService.GetListAsync(page, keyword));
		}
		else
		{
			return BadRequest((await studentService.GetListAsync(page, keyword)));
		}
		
	}
	[HttpGet("ByNewStudents")]
	[Consumes("application/json")]
	[Produces("application/json")]
	public async Task<IActionResult> GetNewStudent([FromQuery(Name = "page")] int page, [FromQuery(Name = "keyword")] string? keyword)
	{
		if (page <= 0) page = 1;

		if ((await studentService.GetListAsync(page, keyword)).status)
		{
			return Ok(await studentService.GetNewStudentsAsync(page, keyword));
		}
		else
		{
			return BadRequest((await studentService.GetListAsync(page, keyword)));
		}

	}
}
