using Castle.Core.Internal;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
	private readonly IRoleService roleService;

	public RolesController(IRoleService roleService)
	{
		this.roleService = roleService;
	}

	[HttpGet]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await roleService.GetListAsync()).status)
			{
				return Ok(await roleService.GetListAsync());
			}
			else
			{
				return NotFound(await roleService.GetListAsync());
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
			if ((await roleService.GetAsync(id)).status)
			{

				return Ok(await roleService.GetAsync(id));
			}
			else
			{
				return NotFound(await roleService.GetAsync(id));
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
			if ((await roleService.GetByNameAsync(name)).status)
			{

				return Ok(await roleService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await roleService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	public async Task<IActionResult> Insert([FromBody] RoleDto roleDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await roleService.InsertAsync(roleDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] RoleDto roleDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await roleService.UpdateAsync(id, roleDto);
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

	[HttpPut("{id}/AddPermissions")]
	public async Task<IActionResult> AddPermission(int id, [FromBody] List<int> permissions)
	{
		try
		{
			if (permissions.IsNullOrEmpty())
			{
				return BadRequest(new { status = false, message = "Failure", error = "Permission Not Null/ Not Empty" });
			}
			var result = await roleService.AddPermissionAsync(id, permissions);
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

	[HttpDelete("{id}/DeletePermissions")]
	public async Task<IActionResult> DeletePermission(int id, [FromBody] List<int> permissions)
	{
		try
		{
			if (permissions.IsNullOrEmpty())
			{
				return BadRequest(new { status = false, message = "Failure", error = "Permission Not Null/ Not Empty" });
			}
			var result = await roleService.DeletePermissionAsync(id, permissions);
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
			var result = await roleService.DeleteAsync(id);
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
