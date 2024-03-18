using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RoomsController : ControllerBase
{

	private readonly IRoomService roomService;
	public RoomsController(IRoomService roomService)
	{
		this.roomService = roomService;
	}

	[HttpGet]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await roomService.GetListAsync()).status)
			{
				return Ok(await roomService.GetListAsync());
			}
			else
			{
				return NotFound(await roomService.GetListAsync());
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
			if ((await roomService.GetAsync(id)).status)
			{

				return Ok(await roomService.GetAsync(id));
			}
			else
			{
				return NotFound(await roomService.GetAsync(id));
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
			if ((await roomService.GetByNameAsync(name)).status)
			{

				return Ok(await roomService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await roomService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Insert([FromBody] RoomDto roomDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await roomService.InsertAsync(roomDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] RoomDto roomDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await roomService.UpdateAsync(id, roomDto);
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

			var result = await roomService.DeleteAsync(id);
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
