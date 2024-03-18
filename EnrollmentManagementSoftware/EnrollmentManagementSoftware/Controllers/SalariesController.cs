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
public class SalariesController : ControllerBase
{
	private readonly ISalaryService salaryService;
	public SalariesController(ISalaryService salaryService)
	{
		this.salaryService = salaryService;
	}
	[HttpGet]
	[Authorize(Policy = "ReadSalaryPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			var salaries = await salaryService.GetListAsync();
			if (salaries.status)
			{
				return Ok(salaries);
			}
			else
			{
				return BadRequest(salaries);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, massage = ex.Message });
		}
	}

	[HttpGet("{id}")]
	[Authorize(Policy = "ReadSalaryPolicy")]
	public async Task<IActionResult> Get(int id)
	{
		try
		{
			var salary = await salaryService.GetAsync(id);
			if (salary.status)
			{
				return Ok(salary);
			}
			else
			{
				return BadRequest(salary);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, massage = ex.Message });
		}
	}
	[HttpPost]
	[Authorize(Policy = "CRUDSalaryPolicy")]
	public async Task<IActionResult> Insert([FromBody] SalaryDto salaryDto)
	{
		try
		{
			var salary = await salaryService.InsertAsync(salaryDto);
			if (salary.status)
			{
				return Ok(salary);
			}
			else
			{
				return BadRequest(salary);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, massage = ex.Message });
		}
	}

	[HttpPut("{id}")]
	[Authorize(Policy = "CRUDSalaryPolicy")]
	public async Task<IActionResult> Update([FromQuery(Name = "status")] bool status, int id)
	{
		try
		{
			var salary = await salaryService.UpdateAsync(id, status);
			if (salary.status)
			{
				return Ok(salary);
			}
			else
			{
				return BadRequest(salary);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, massage = ex.Message });
		}
	}
}
