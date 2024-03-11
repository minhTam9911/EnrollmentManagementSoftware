using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace EnrollmentManagementSoftware.Services.Implements;

public class AuthService : IAuthService
{
	private DatabaseContext dbContext;
	private IHttpContextAccessor httpContextAccessor;
	private IConfiguration configuration;
	public AuthService(DatabaseContext dbContext, IHttpContextAccessor httpContextAccessor,IConfiguration configuration)
	{
		this.dbContext = dbContext;
		this.httpContextAccessor = httpContextAccessor;
		this.configuration = configuration;
	}

	public Task<dynamic> ChangeForgotPassword(ResetPasswordDto resetPasswordDto)
	{
		throw new NotImplementedException();
	}

	public Task<dynamic> ChangePassword(ChangePasswordDto changePasswordDto)
	{
		throw new NotImplementedException();
	}

	public Task<dynamic> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
	{
		throw new NotImplementedException();
	}

	public Task<dynamic> LoginAsync(AccountDto accountDto)
	{
		throw new NotImplementedException();
	}

	public Task<dynamic> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
	{
		throw new NotImplementedException();
	}

	public Task<dynamic> RevokeTokenAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task<dynamic> VerifySecurityCode(VerifySecurityCodeDto verifySecurityCodeDto)
	{
		throw new NotImplementedException();
	}

	private string GenerateJwtToken(Guid id, string role)
	{
		List<Claim> claims = new List<Claim> {

			new Claim(ClaimTypes.Name, id.ToString()),
			new Claim(ClaimTypes.Role, role)

		};
		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
			configuration.GetSection("AppSettings:Token").Value!));
		var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
		var token = new JwtSecurityToken(
			claims: claims,
			expires: DateTime.Now.AddHours(1),
			signingCredentials: creds

			);

		var jwt = new JwtSecurityTokenHandler().WriteToken(token);

		return jwt;
	}
	private RefreshTokenDto GenerateRefreshToken()
	{
		var expires = DateTime.Now.AddDays(7);
		var more = Guid.NewGuid().ToString().Replace("-", "");
		var token = more + Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		var cookieOption = new CookieOptions
		{
			HttpOnly = true,
			Expires = expires,

		};

		httpContextAccessor.HttpContext!.Response.Cookies.Append("resfreshToken", token, cookieOption);
		var apiToken = new RefreshTokenDto
		{
			RefreshToken = token,
			ExipresRefreshToken = expires
		};
		return apiToken;
	}
}
