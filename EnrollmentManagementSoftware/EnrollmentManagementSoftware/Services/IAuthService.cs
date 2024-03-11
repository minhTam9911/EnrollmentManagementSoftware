using EnrollmentManagementSoftware.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Services;

public interface IAuthService
{
	Task<dynamic> LoginAsync(AccountDto accountDto);
	Task<dynamic> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
	Task<dynamic> RevokeTokenAsync(Guid id);
	Task<dynamic> ChangePassword(ChangePasswordDto changePasswordDto);
	Task<dynamic> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
	Task<dynamic> VerifySecurityCode(VerifySecurityCodeDto verifySecurityCodeDto);
	Task<dynamic> ChangeForgotPassword(ResetPasswordDto resetPasswordDto);

}
