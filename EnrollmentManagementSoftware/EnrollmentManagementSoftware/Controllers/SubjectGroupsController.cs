using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using EnrollmentManagementSoftware.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class SubjectGroupsController : ControllerBase
{
	private readonly ISubjectGroupService subjectGroupService;
	public SubjectGroupsController(ISubjectGroupService subjectGroupService)
	{
		this.subjectGroupService = subjectGroupService;
	}

	[HttpGet]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await subjectGroupService.GetListAsync()).status)
			{
				return Ok(await subjectGroupService.GetListAsync());
			}
			else
			{
				return NotFound(await subjectGroupService.GetListAsync());
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}

	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id)
	{
		try
		{
			if ((await subjectGroupService.GetAsync(id)).status)
			{

				return Ok(await subjectGroupService.GetAsync(id));
			}
			else
			{
				return NotFound(await subjectGroupService.GetAsync(id));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}
	[HttpGet("Detail/{id}")]
	public async Task<IActionResult> GetDetail(int id)
	{
		try
		{
			if ((await subjectGroupService.GetDetailAsync(id)).status)
			{

				return Ok(await subjectGroupService.GetDetailAsync(id));
			}
			else
			{
				return NotFound(await subjectGroupService.GetDetailAsync(id));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("ByName/{name}")]
	public async Task<IActionResult> GetByName(string name)
	{
		try
		{
			if ((await subjectGroupService.GetByNameAsync(name)).status)
			{

				return Ok(await subjectGroupService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await subjectGroupService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	public async Task<IActionResult> Insert([FromBody] SubjectGroupDto subjectGroupDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await subjectGroupService.InsertAsync(subjectGroupDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] SubjectGroupDto subjectGroupDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await subjectGroupService.UpdateAsync(id, subjectGroupDto);
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
	public async Task<IActionResult> Delete(int id)
	{
		try
		{

			var result = await subjectGroupService.DeleteAsync(id);
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
