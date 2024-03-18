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
public class UsersController : ControllerBase
{
	private readonly IUserService userService;
	public UsersController(IUserService userService)
	{
		this.userService = userService;
	}
	[HttpGet]
	[Authorize(Policy = "ReadUserPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await userService.GetListAsync()).status)
			{
				return Ok(await userService.GetListAsync());
			}
			else
			{
				return BadRequest(await userService.GetListAsync());
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("{id}")]
	[Authorize(Policy = "ReadUserPolicy")]
	public async Task<IActionResult> Get(Guid id)
	{
		try
		{
			if ((await userService.GetAsync(id)).status)
			{

				return Ok(await userService.GetAsync(id));
			}
			else
			{
				return BadRequest(await userService.GetAsync(id));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("ByName/{name}")]
	[Authorize(Policy = "ReadUserPolicy")]
	public async Task<IActionResult> GetByName(string name)
	{
		try
		{
			if ((await userService.GetByNameAsync(name)).status)
			{

				return Ok(await userService.GetByNameAsync(name));
			}
			else
			{
				return BadRequest(await userService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Insert([FromForm] UserDto userDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await userService.InsertAsync(userDto);
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
	public async Task<IActionResult> Update(Guid id, [FromForm] UserDto userDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await userService.UpdateAsync(id, userDto);
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
	public async Task<IActionResult> Delete(Guid id)
	{
		try
		{

			var result = await userService.DeleteAsync(id);
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
