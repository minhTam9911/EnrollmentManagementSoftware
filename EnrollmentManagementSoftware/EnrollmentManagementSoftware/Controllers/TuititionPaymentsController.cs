using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TuititionPaymentsController : ControllerBase
{

	private readonly ITuititionPaymentService tuititionPaymentService;
	public TuititionPaymentsController(ITuititionPaymentService tuititionPaymentService)
	{
		this.tuititionPaymentService = tuititionPaymentService;
	}

	[HttpGet]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> GetList()
	{
		try
		{
			var tuititionPayments = await tuititionPaymentService.GetListAsync();
			if (tuititionPayments.status)
			{
				return Ok(tuititionPayments);
			}
			else
			{
				return BadRequest(tuititionPayments);
			}
		}
		catch(Exception ex)
		{
			return BadRequest(new {status = false, massage = ex.Message});
		}
	}

	[HttpGet("{id}")]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Get(Guid id)
	{
		try
		{
			var tuititionPayment = await tuititionPaymentService.GetAsync(id);
			if (tuititionPayment.status)
			{
				return Ok(tuititionPayment);
			}
			else
			{
				return BadRequest(tuititionPayment);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, massage = ex.Message });
		}
	}
	[HttpPost]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Insert([FromBody] TuitionPaymentDto tuitionPaymentDto)
	{
		try
		{
			var tuititionPayment = await tuititionPaymentService.InsertAsync(tuitionPaymentDto);
			if (tuititionPayment.status)
			{
				return Ok(tuititionPayment);
			}
			else
			{
				return BadRequest(tuititionPayment);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, massage = ex.Message });
		}
	}

	[HttpPut("{id}")]
	[Authorize(Policy = "AdminPolicy")]
	public async Task<IActionResult> Update([FromQuery(Name ="status")] bool status, Guid id)
	{
		try
		{
			var tuititionPayment = await tuititionPaymentService.UpdateAsync(id,status);
			if (tuititionPayment.status)
			{
				return Ok(tuititionPayment);
			}
			else
			{
				return BadRequest(tuititionPayment);
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, massage = ex.Message });
		}
	}
}
