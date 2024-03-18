using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SchedulesController : ControllerBase
{
	private readonly IScheduleService scheduleService;
	public SchedulesController(IScheduleService scheduleService)
	{
		this.scheduleService = scheduleService;
	}

	[HttpGet]
	[Authorize(Policy = "ReadSchedulePolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await scheduleService.GetListAsync()).status)
			{
				return Ok(await scheduleService.GetListAsync());
			}
			else
			{
				return NotFound(await scheduleService.GetListAsync());
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("{id}")]
	[Authorize(Policy = "ReadSchedulePolicy")]
	public async Task<IActionResult> Get(int id)
	{
		try
		{
			if ((await scheduleService.GetAsync(id)).status)
			{

				return Ok(await scheduleService.GetAsync(id));
			}
			else
			{
				return NotFound(await scheduleService.GetAsync(id));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("ByTeacher/{teacherId}")]
	[Authorize(Policy = "ReadSchedulePolicy")]
	public async Task<IActionResult> GetByTeacher(Guid teacherId)
	{
		try
		{
			if ((await scheduleService.GetByTeacherAsync(teacherId)).status)
			{

				return Ok(await scheduleService.GetByTeacherAsync(teacherId));
			}
			else
			{
				return NotFound(await scheduleService.GetByTeacherAsync(teacherId));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpGet("ByStudent/{studentId}")]
	[Authorize(Policy = "ReadSchedulePolicy")]
	public async Task<IActionResult> GetByStudent(Guid studentId)
	{
		try
		{
			if ((await scheduleService.GetByStudentAsync(studentId)).status)
			{

				return Ok(await scheduleService.GetByStudentAsync(studentId));
			}
			else
			{
				return NotFound(await scheduleService.GetByStudentAsync(studentId));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}



	[HttpPost]
	[Authorize(Policy = "CRUDSchedulePolicy")]
	public async Task<IActionResult> Insert([FromBody] ScheduleDto scheduleDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await scheduleService.InsertAsync(scheduleDto);
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
	[Authorize(Policy = "CRUDSchedulePolicy")]
	public async Task<IActionResult> Update(int id, [FromBody] ScheduleDto scheduleDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await scheduleService.UpdateAsync(id, scheduleDto);
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
	[Authorize(Policy = "CRUDSchedulePolicy")]
	public async Task<IActionResult> Delete(int id)
	{
		try
		{

			var result = await scheduleService.DeleteAsync(id);
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
