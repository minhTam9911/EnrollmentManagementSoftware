using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
	// GET: api/<TestController>
	[HttpPost]
	public IActionResult Get([FromForm] UserDto user)
	{
		if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}
		else
		{
			return Ok();
		}
	}
}
