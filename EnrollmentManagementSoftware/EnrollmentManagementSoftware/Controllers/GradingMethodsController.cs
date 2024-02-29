using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using EnrollmentManagementSoftware.Services.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GradingMethodsController : ControllerBase
{
	private readonly IGrandingMethodService grandingMethodService;
	public GradingMethodsController(IGrandingMethodService grandingMethodService)
	{
		this.grandingMethodService = grandingMethodService;
	}

	[HttpGet]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await grandingMethodService.GetListAsync()).status)
			{
				return Ok(await grandingMethodService.GetListAsync());
			}
			else
			{
				return NotFound(await grandingMethodService.GetListAsync());
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
			if ((await grandingMethodService.GetAsync(id)).status)
			{

				return Ok(await grandingMethodService.GetAsync(id));
			}
			else
			{
				return NotFound(await grandingMethodService.GetAsync(id));
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
			if ((await grandingMethodService.GetByNameAsync(name)).status)
			{

				return Ok(await grandingMethodService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await grandingMethodService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	public async Task<IActionResult> Insert([FromBody] GradingMethodDto gradingMethodDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await grandingMethodService.InsertAsync(gradingMethodDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] GradingMethodDto gradingMethodDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await grandingMethodService.UpdateAsync(id, gradingMethodDto);
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
			
			var result = await grandingMethodService.DeleteAsync(id);
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
