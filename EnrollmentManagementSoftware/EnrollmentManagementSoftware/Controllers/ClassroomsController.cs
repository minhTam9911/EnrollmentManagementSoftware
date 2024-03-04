using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ClassroomsController : ControllerBase
{
	private readonly IClassroomService classroomService;
	public ClassroomsController(IClassroomService classroomService)
	{
		this.classroomService = classroomService;
	}

	[HttpGet]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await classroomService.GetListAsync()).status)
			{
				return Ok(await classroomService.GetListAsync());
			}
			else
			{
				return NotFound(await classroomService.GetListAsync());
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
			if ((await classroomService.GetAsync(id)).status)
			{

				return Ok(await classroomService.GetAsync(id));
			}
			else
			{
				return NotFound(await classroomService.GetAsync(id));
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
			if ((await classroomService.GetByNameAsync(name)).status)
			{

				return Ok(await classroomService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await classroomService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	public async Task<IActionResult> Insert([FromBody] ClassroomDto classroomDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await classroomService.InsertAsync(classroomDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] ClassroomDto classroomDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await classroomService.UpdateAsync(id, classroomDto);
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

			var result = await classroomService.DeleteAsync(id);
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
