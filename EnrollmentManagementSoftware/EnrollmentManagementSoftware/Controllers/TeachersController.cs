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
public class TeachersController : ControllerBase
{

	private readonly ITeacherService teacherService;
	public TeachersController(ITeacherService teacherService)
	{
		this.teacherService = teacherService;
	}


	[HttpGet]
	[Authorize(Policy = "ReadTeacherPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await teacherService.GetListAsync()).status)
			{
				return Ok(await teacherService.GetListAsync());
			}
			else
			{
				return NotFound(await teacherService.GetListAsync());
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("{id}")]
	[Authorize(Policy = "ReadTeacherPolicy")]
	public async Task<IActionResult> Get(Guid id)
	{
		try
		{
			if ((await teacherService.GetAsync(id)).status)
			{

				return Ok(await teacherService.GetAsync(id));
			}
			else
			{
				return NotFound(await teacherService.GetAsync(id));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("ByName/{name}")]
	[Authorize(Policy = "ReadTeacherPolicy")]
	public async Task<IActionResult> GetByName(string name)
	{
		try
		{
			if ((await teacherService.GetByNameAsync(name)).status)
			{

				return Ok(await teacherService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await teacherService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	[Authorize(Policy = "CRUDTeacherPolicy")]
	public async Task<IActionResult> Insert([FromForm] TeacherDto teacherDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await teacherService.InsertAsync(teacherDto);
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
	[Authorize(Policy = "CRUDTeacherPolicy")]
	public async Task<IActionResult> Update(Guid id, [FromBody] TeacherDto teacherDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await teacherService.UpdateAsync(id, teacherDto);
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
	[Authorize(Policy = "CRUDTeacherPolicy")]
	public async Task<IActionResult> Delete(Guid id)
	{
		try
		{

			var result = await teacherService.DeleteAsync(id);
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
