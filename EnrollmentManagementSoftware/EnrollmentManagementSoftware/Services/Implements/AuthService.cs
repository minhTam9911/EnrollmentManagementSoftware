using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Helplers;
using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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
	public AuthService(DatabaseContext dbContext, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
	{
		this.dbContext = dbContext;
		this.httpContextAccessor = httpContextAccessor;
		this.configuration = configuration;
	}

	public async Task<dynamic> ChangeForgotPasswordAsync(ResetPasswordDto resetPasswordDto)
	{
		try
		{
			var account = await dbContext.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == resetPasswordDto.Token);
			if (account != null)
			{
				if (account.ResetTokenExpires > DateTime.Now)
				{
					if (resetPasswordDto.NewPassword == resetPasswordDto.ConfirmPassword)
					{
						var hashPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
						account.Password = hashPassword;
						account.PasswordResetToken = null;
						account.ResetTokenExpires = null;
						dbContext.Entry(account).State = EntityState.Modified;
						if (await dbContext.SaveChangesAsync() > 0)
						{
							return new { status = true, message = "Ok" };
						}
						else
						{
							return new { status = false, message = "Failure" };
						}
					}
					else
					{
						return new { status = false, message = "The New Password Does Not Match The Confirm Password" };
					}

				}
				else
				{
					return new { status = false, message = "Token Has Expired" };
				}
			}
			else
			{
				return new { status = false, message = "Token Does Not Exist" };
			}

		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
	{
		try
		{
			if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
			{
				return new { status = false, message = "The New Password Does Not Match The Confirm Password" };
			}
			var account = await dbContext.Users.FindAsync(Guid.Parse(httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.Name).ToString()));
			if (account != null)
			{
				if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, account.Password))
				{
					return new { status = false, message = "The Old Password Does Not Match The New Password" };
				}
				var hashPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
				account.Password = hashPassword;
				dbContext.Entry(account).State = EntityState.Modified;
				if (await dbContext.SaveChangesAsync() > 0)
				{
					return new { status = true, message = "Ok" };
				}
				else
				{
					return new { status = false, message = "Failure" };
				}
			}
			return new { status = false, message = "User Not Found" };
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
	{
		try
		{
			var account = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == forgotPasswordDto.Email.ToLower());
			if (account != null)
			{
				account.PasswordResetToken = GeneratePasswordResetToken();
				account.ResetTokenExpires = DateTime.Now.AddMinutes(10);
				account.SecurityCode = RandomHelper.RandomInt(6);
				var mailHelper = new MailHelper(configuration);
				mailHelper.Send(configuration["Gmail:Username"], account.Email, "Verification Code", MailHelper.HtmlVerify(account.SecurityCode));
				dbContext.Entry(account).State = EntityState.Modified;
				if (await dbContext.SaveChangesAsync() > 0)
				{
					return new { status = true, message = "Ok", token = account.PasswordResetToken };
				}
				else
				{
					return new { status = false, message = "Failure" };
				}
			}
			return new { status = false, message = "User Not Found" };
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> LoginAsync(AccountDto accountDto)
	{
		try
		{
			var account = await dbContext.Users.FirstOrDefaultAsync(x => x.Email == accountDto.Username.ToLower());
			if (account != null)
			{

				if (BCrypt.Net.BCrypt.Verify(accountDto.Password, account.Password))
				{
					if ((bool)account.IsStatus)
					{
						var accessToken = GenerateJwtToken(account.Id, account.Role.Name);
						var refreshToken = GenerateRefreshToken();
						account.RefreshToken = refreshToken.RefreshToken;
						account.RefreshTokenExipres = refreshToken.ExipresRefreshToken;
						dbContext.Entry(account).State = EntityState.Modified;
						if (await dbContext.SaveChangesAsync() > 0)
						{
							return new
							{
								status = true,
								message = "Ok",
								accessToken = accessToken,
								refreshToken = refreshToken.RefreshToken
							};
						}
						else
						{
							return new { status = false, message = "Failure" };
						}
					}
					else
					{
						account.PasswordResetToken = GeneratePasswordResetToken();
						account.ResetTokenExpires = DateTime.Now.AddMinutes(10);
						account.SecurityCode = RandomHelper.RandomInt(6);
						var mailHelper = new MailHelper(configuration);
						mailHelper.Send(configuration["Gmail:Username"], account.Email, "Verification Code", MailHelper.HtmlVerify(account.SecurityCode));
						dbContext.Entry(account).State = EntityState.Modified;
						if (await dbContext.SaveChangesAsync() > 0)
						{
							return new { status = false, message = "Account No Active. Please Check Your Email", token = account.PasswordResetToken };
						}
						else
						{
							return new { status = false, message = "Failure" };
						}
					}
				}

				else
				{
					return new { status = false, message = "Password Does Not Match" };
				}


			}
			else
			{
				return new { status = false, message = "Username Not Found" };
			}
			
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
	{
		var account = await dbContext.Users.FirstOrDefaultAsync(x => x.RefreshToken == refreshTokenDto.RefreshToken);
		if (account != null)
		{
			if (account.RefreshTokenExipres > DateTime.Now)
			{
				var accessToken = GenerateJwtToken(account.Id, account.Role.Name);
				return new
				{
					status = true,
					message = "Ok",
					accessToken = accessToken,
					refreshToken = account.RefreshToken
				};
			}
			else
			{
				return new { status = false, message = "Resfresh Token Has Expired" };
			}
		}
		else
		{
			return new { status = false, message = "Resfresh Token Does Not Exist. Please Log In" };
		}
	}

	public async Task<dynamic> RevokeTokenAsync(Guid? id)
	{

		var account = await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
		if (account != null)
		{
			account.RefreshTokenExipres = null;
			account.RefreshTokenExipres = null;
			dbContext.Entry(account).State = EntityState.Modified;
			await dbContext.SaveChangesAsync();
			return new
			{
				status = true,
				message = "Ok",
				accessToken = string.Empty,
				refreshToken = string.Empty
			};
		}
		else
		{
			return new { status = false, message = "User Not Found" };
		}
	}

	public async Task<dynamic> VerifySecurityCodeAsync(VerifySecurityCodeDto verifySecurityCodeDto)
	{
		try
		{
			var account = await dbContext.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == verifySecurityCodeDto.Token);
			if (account != null)
			{
				if (account.ResetTokenExpires > DateTime.Now)
				{
					if (account.SecurityCode == verifySecurityCodeDto.Code)
					{
						account.SecurityCode = null;
						dbContext.Entry(account).State = EntityState.Modified;
						if (await dbContext.SaveChangesAsync() > 0)
						{
							return new { status = true, message = "Ok" };
						}
						else
						{
							return new { status = false, message = "Failure" };
						}
					}
					else
					{

						return new { status = false, message = "Verification Code Does Not Match" };
					}
				}
				else
				{
					return new { status = false, message = "Token Has Expired" };
				}
			}
			else
			{
				return new { status = false, message = "Token Does Not Exist" };
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
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
			issuer: configuration.GetSection("AppSettings:ValidIssuer").Value,
			audience: configuration.GetSection("AppSettings:ValidAudience").Value,
			claims: claims,
			expires: DateTime.Now.AddMinutes(15),
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
	private string GeneratePasswordResetToken()
	{
		return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
	}

	public async Task<dynamic> ActiveAccountAsync(VerifySecurityCodeDto verifySecurityCodeDto)
	{
		try
		{
			var account = await dbContext.Users.FirstOrDefaultAsync(x => x.PasswordResetToken == verifySecurityCodeDto.Token);
			if (account != null)
			{
				if (account.ResetTokenExpires > DateTime.Now)
				{
					if (account.SecurityCode == verifySecurityCodeDto.Code)
					{
						account.SecurityCode = null;
						account.PasswordResetToken = null;
						account.ResetTokenExpires = null;
						account.IsStatus = true;
						dbContext.Entry(account).State = EntityState.Modified;
						if (await dbContext.SaveChangesAsync() > 0)
						{
							return new { status = true, message = "Ok" };
						}
						else
						{
							return new { status = false, message = "Failure" };
						}
					}
					else
					{

						return new { status = false, message = "Verification Code Does Not Match" };
					}
				}
				else
				{
					return new { status = false, message = "Token Has Expired" };
				}
			}
			else
			{
				return new { status = false, message = "Token Does Not Exist" };
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}
}
