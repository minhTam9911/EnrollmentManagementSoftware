﻿using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TuitionTypesController : ControllerBase
{
	private readonly ITuitionTypeService tuitionTypeService;
	public TuitionTypesController(ITuitionTypeService tuitionTypeService)
	{
		this.tuitionTypeService = tuitionTypeService;
	}

	[HttpGet]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await tuitionTypeService.GetListAsync()).status)
			{
				return Ok(await tuitionTypeService.GetListAsync());
			}
			else
			{
				return BadRequest(await tuitionTypeService.GetListAsync());
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
			if ((await tuitionTypeService.GetAsync(id)).status)
			{

				return Ok(await tuitionTypeService.GetAsync(id));
			}
			else
			{
				return BadRequest(await tuitionTypeService.GetAsync(id));
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
			if ((await tuitionTypeService.GetByNameAsync(name)).status)
			{

				return Ok(await tuitionTypeService.GetByNameAsync(name));
			}
			else
			{
				return BadRequest(await tuitionTypeService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}


	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Insert([FromBody] TuitionTypeDto tuitionTypeDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await tuitionTypeService.InsertAsync(tuitionTypeDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] TuitionTypeDto tuitionTypeDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await tuitionTypeService.UpdateAsync(id,tuitionTypeDto);
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

			var result = await tuitionTypeService.DeleteAsync(id);
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
