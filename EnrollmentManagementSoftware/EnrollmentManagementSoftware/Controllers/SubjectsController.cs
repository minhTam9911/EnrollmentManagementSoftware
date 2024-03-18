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
public class SubjectsController : ControllerBase
{
	private readonly ISubjectService subjectService;
	public SubjectsController(ISubjectService subjectService)
	{
		this.subjectService = subjectService;
	}

	[HttpGet]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await subjectService.GetListAsync()).status)
			{
				return Ok(await subjectService.GetListAsync());
			}
			else
			{
				return BadRequest(await subjectService.GetListAsync());
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("{id}")]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Get(int id)
	{
		try
		{
			if ((await subjectService.GetAsync(id)).status)
			{

				return Ok(await subjectService.GetAsync(id));
			}
			else
			{
				return BadRequest(await subjectService.GetAsync(id));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("ByName/{name}")]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> GetByName(string name)
	{
		try
		{
			if ((await subjectService.GetByNameAsync(name)).status)
			{

				return Ok(await subjectService.GetByNameAsync(name));
			}
			else
			{
				return BadRequest(await subjectService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Insert([FromBody] SubjectDto subjectDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await subjectService.InsertAsync(subjectDto);
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
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Update(int id, [FromBody] SubjectDto subjectDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await subjectService.UpdateAsync(id, subjectDto);
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
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{

			var result = await subjectService.DeleteAsync(id);
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
