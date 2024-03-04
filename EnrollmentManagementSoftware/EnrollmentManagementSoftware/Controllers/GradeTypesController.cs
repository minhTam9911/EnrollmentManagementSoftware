using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class GradeTypesController : ControllerBase
{
	private readonly IGrandeTypeService grandeTypeService;
	public GradeTypesController(IGrandeTypeService  grandeTypeService)
	{
		this.grandeTypeService = grandeTypeService;
	}

	[HttpGet]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await grandeTypeService.GetListAsync()).status)
			{
				return Ok(await grandeTypeService.GetListAsync());
			}
			else
			{
				return NotFound(await grandeTypeService.GetListAsync());
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
			if ((await grandeTypeService.GetAsync(id)).status)
			{

				return Ok(await grandeTypeService.GetAsync(id));
			}
			else
			{
				return NotFound(await grandeTypeService.GetAsync(id));
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
			if ((await grandeTypeService.GetByNameAsync(name)).status)
			{

				return Ok(await grandeTypeService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await grandeTypeService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	public async Task<IActionResult> Insert([FromBody] GradeTypeDto gradeTypeDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await grandeTypeService.InsertAsync(gradeTypeDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] GradeTypeDto gradeTypeDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await grandeTypeService.UpdateAsync(id,gradeTypeDto);
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

			var result = await grandeTypeService.DeleteAsync(id);
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
