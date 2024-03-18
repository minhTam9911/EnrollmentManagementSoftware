using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class GradesController : ControllerBase
{
	private readonly IGradeService gradeService;
	public GradesController(IGradeService gradeService)
	{
		this.gradeService = gradeService;
	}

	[HttpGet]
	[Authorize(Policy = "ReadGradePolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await gradeService.GetListAsync()).status)
			{
				return Ok(await gradeService.GetListAsync());
			}
			else
			{
				return NotFound(await gradeService.GetListAsync());
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}

	}

	[HttpGet("{id}")]
	[Authorize(Policy = "ReadGradePolicy")]
	public async Task<IActionResult> Get(int id)
	{
		try
		{
			if ((await gradeService.GetAsync(id)).status)
			{

				return Ok(await gradeService.GetAsync(id));
			}
			else
			{
				return NotFound(await gradeService.GetAsync(id));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("ByName/{name}")]
	[Authorize(Policy = "ReadGradePolicy")]
	public async Task<IActionResult> GetByName(string name)
	{
		try
		{
			if ((await gradeService.GetByNameAsync(name)).status)
			{

				return Ok(await gradeService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await gradeService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	[Authorize(Policy = "CreateGradePolicy")]
	public async Task<IActionResult> Insert([FromBody] GradeDto gradeDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await gradeService.InsertAsync(gradeDto);
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
	[Authorize(Policy = "UpdateGradePolicy")]
	public async Task<IActionResult> Update(int id, [FromBody] GradeDto gradeDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await gradeService.UpdateAsync(id, gradeDto);
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
	[Authorize(Policy = "CRUDGradeTypePolicy")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{

			var result = await gradeService.DeleteAsync(id);
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
