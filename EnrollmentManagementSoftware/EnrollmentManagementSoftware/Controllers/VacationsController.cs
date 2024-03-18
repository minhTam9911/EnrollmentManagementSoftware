using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class VacationsController : ControllerBase
{
	private readonly IVacationService vacationService;
	public VacationsController(IVacationService vacationService)
	{
		this.vacationService = vacationService;
	}

	[HttpGet]
	[Authorize(Policy = "ReadVacationPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await vacationService.GetListAsync()).status)
			{
				return Ok(await vacationService.GetListAsync());
			}
			else
			{
				return NotFound(await vacationService.GetListAsync());
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}

	}

	[HttpGet("{id}")]
	[Authorize(Policy = "ReadVacationPolicy")]
	public async Task<IActionResult> Get(int id)
	{
		try
		{
			if ((await vacationService.GetAsync(id)).status)
			{

				return Ok(await vacationService.GetAsync(id));
			}
			else
			{
				return NotFound(await vacationService.GetAsync(id));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("ByName/{name}")]
	[Authorize(Policy = "ReadVacationPolicy")]
	public async Task<IActionResult> GetByName(string name)
	{
		try
		{
			if ((await vacationService.GetByNameAsync(name)).status)
			{

				return Ok(await vacationService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await vacationService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	[Authorize(Policy = "CRUDVacationPolicy")]
	public async Task<IActionResult> Insert([FromBody] VacationDto vacationDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await vacationService.InsertAsync(vacationDto);
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
	[Authorize(Policy = "CRUDVacationPolicy")]
	public async Task<IActionResult> Update(int id, [FromBody] VacationDto vacationDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await vacationService.UpdateAsync(id,vacationDto);
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
	[Authorize(Policy = "CRUDVacationPolicy")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{

			var result = await vacationService.DeleteAsync(id);
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
