using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using EnrollmentManagementSoftware.Services.Implements;
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

	[HttpPost]
	public async Task<IActionResult> Insert([FromBody] StudentDto studentDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await studentService.InsertAsync(studentDto);
			if (result.status)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}

	}


	[HttpPut("{id}")]
	public async Task<IActionResult> Update(Guid id, [FromBody] StudentDto studentDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await studentService.UpdateAsync(id, studentDto);
			if (result.status)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		try
		{

			var result = await studentService.DeleteAsync(id);
			if (result.status)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(result);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

}
