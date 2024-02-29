using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CoursesController : ControllerBase
{
	private readonly ICourseService courseService;
	public CoursesController(ICourseService courseService)
	{
		this.courseService = courseService;
	}

	[HttpGet]
	public async Task<IActionResult> GetList()
	{
		try
		{
			if ((await courseService.GetListAsync()).status) {
				return Ok(await courseService.GetListAsync());
			}
			else
			{
				return NotFound(await courseService.GetListAsync());
			}
		} catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}

	}

	[HttpGet("{id}")]
	public async Task<IActionResult> Get(int id)
	{
		try
		{
			if((await courseService.GetAsync(id)).status) {

				return Ok(await courseService.GetAsync(id));
			}
			else
			{
				return NotFound(await courseService.GetAsync(id));
			}
		}catch(Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}
	[HttpGet("Detail/{id}")]
	public async Task<IActionResult> GetDetail(int id)
	{
		try
		{
			if ((await courseService.GetDetailAsync(id)).status)
			{

				return Ok(await courseService.GetDetailAsync(id));
			}
			else
			{
				return NotFound(await courseService.GetDetailAsync(id));
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
			if ((await courseService.GetByNameAsync(name)).status)
			{

				return Ok(await courseService.GetByNameAsync(name));
			}
			else
			{
				return NotFound(await courseService.GetByNameAsync(name));
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	
	[HttpPost]
	public async Task<IActionResult> Insert([FromBody] CourseDto courseDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await courseService.InsertAsync(courseDto);
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
	public async Task<IActionResult> Update(int id, [FromBody] CourseDto courseDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = "Failure", error = ModelState });
			}
			var result = await courseService.UpdateAsync(id,courseDto);
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
			
			var result = await courseService.DeleteAsync(id);
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
