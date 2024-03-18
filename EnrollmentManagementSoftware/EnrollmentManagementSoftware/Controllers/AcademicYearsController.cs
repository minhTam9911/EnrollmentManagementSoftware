using Castle.Core.Internal;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AcademicYearsController : ControllerBase
{
	private readonly IAcademicYearService academicYearService;
	public AcademicYearsController(IAcademicYearService academicYearService)
	{
		this.academicYearService = academicYearService;
	}

	[HttpGet]
	[Authorize(Policy ="AdminPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await academicYearService.GetListAsync()).status)
			{
				return Ok(await academicYearService.GetListAsync());
			}
			else
			{
				return BadRequest(await academicYearService.GetListAsync());
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
			if ((await academicYearService.GetAsync(id)).status)
			{

				return Ok(await academicYearService.GetAsync(id));
			}
			else
			{
				return BadRequest(await academicYearService.GetAsync(id));
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
			if ((await academicYearService.GetByNameAsync(name)).status)
			{

				return Ok(await academicYearService.GetByNameAsync(name));
			}
			else
			{
				return BadRequest(await academicYearService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Insert([FromBody] AcademicYearDto academicYearDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await academicYearService.InsertAsync(academicYearDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] AcademicYearDto academicYearDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await academicYearService.UpdateAsync(id,academicYearDto);
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

			var result = await academicYearService.DeleteAsync(id);
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
