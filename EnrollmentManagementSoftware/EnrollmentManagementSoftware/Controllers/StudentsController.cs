using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using EnrollmentManagementSoftware.Services.Implements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StudentController : ControllerBase
{
	private readonly IStudentService studentService;
	public StudentController(IStudentService studentService)
	{
		this.studentService = studentService;
	}
	
	[HttpGet]
	[Authorize(Policy = "ReadStudentPolicy")]
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
	[HttpGet("{id}")]
	[Authorize(Policy = "ReadStudentPolicy")]
	public async Task<IActionResult> GetNewStudent(Guid id)
	{

		if ((await studentService.GetStudentDetailsAsync(id)).status)
		{
			return Ok(await studentService.GetStudentDetailsAsync(id));
		}
		else
		{
			return BadRequest(await studentService.GetStudentDetailsAsync(id));
		}

	}

	[HttpPost]
	[Authorize(Policy = "CRUDStudentPolicy")]
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
	[Authorize(Policy = "CRUDStudentPolicy")]
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
	[Authorize(Policy = "CRUDStudentPolicy")]
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
