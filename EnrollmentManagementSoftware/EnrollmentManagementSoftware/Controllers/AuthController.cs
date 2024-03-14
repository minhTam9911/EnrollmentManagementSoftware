using Castle.Core.Internal;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private IAuthService authService;
	public AuthController(IAuthService authService)
	{
		this.authService = authService;
	}

	[HttpPost("login")]
	public async Task<IActionResult> Login([FromBody] AccountDto accountDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = ModelState });
			}
			else
			{
				var auth = await authService.LoginAsync(accountDto);
				if (auth.status)
				{
					return Ok(auth);
				}
				else
				{
					return BadRequest(auth);
				}
			}
		}catch (Exception ex)
		{
			return BadRequest(new {status = false, message =  ex.Message });
		}
	}

	[HttpPost("refeshToken")]
	public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = ModelState });
			}
			else
			{
				var auth = await authService.RefreshTokenAsync(refreshTokenDto);
				if (auth.status)
				{
					return Ok(auth);
				}
				else
				{
					return BadRequest(auth);
				}
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpPost("revoke")]
	[Authorize]
	public async Task<IActionResult> Revoke()
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = ModelState });
			}
			else
			{
				var auth = await authService.RevokeTokenAsync(Guid.Parse(User?.Identity.Name));
				if (auth.status)
				{
					return Ok(auth);
				}
				else
				{
					return BadRequest(auth);
				}
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}
	[HttpPost("changePassword")]
	[Authorize]
	public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDto changePasswordDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = ModelState });
			}
			else
			{
				var auth = await authService.ChangePasswordAsync(changePasswordDto);
				if (auth.status)
				{
					return Ok(auth);
				}
				else
				{
					return BadRequest(auth);
				}
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpPost("forgotPassword")]
	public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = ModelState });
			}
			else
			{
				var auth = await authService.ForgotPasswordAsync(forgotPasswordDto);
				if (auth.status)
				{
					return Ok(auth);
				}
				else
				{
					return BadRequest(auth);
				}
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpPost("verifySecutiryCode")]
	public async Task<IActionResult> VerifySecurityCode([FromBody] VerifySecurityCodeDto verifySecurityCodeDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = ModelState });
			}
			else
			{
				var auth = await authService.VerifySecurityCodeAsync(verifySecurityCodeDto);
				if (auth.status)
				{
					return Ok(auth);
				}
				else
				{
					return BadRequest(auth);
				}
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

	[HttpPost("changeForgotPassword")]
	public async Task<IActionResult> changeForgotPassword([FromBody] ResetPasswordDto resetPasswordDto)
	{
		try
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(new { status = false, message = ModelState });
			}
			else
			{
				var auth = await authService.ChangeForgotPasswordAsync(resetPasswordDto);
				if (auth.status)
				{
					return Ok(auth);
				}
				else
				{
					return BadRequest(auth);
				}
			}
		}
		catch (Exception ex)
		{
			return BadRequest(new { status = false, message = ex.Message });
		}
	}

}
