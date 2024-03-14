using EnrollmentManagementSoftware.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnrollmentManagementSoftware.Services;

public interface IAuthService
{
	Task<dynamic> LoginAsync(AccountDto accountDto);
	Task<dynamic> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
	Task<dynamic> RevokeTokenAsync(Guid? id);
	Task<dynamic> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
	Task<dynamic> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
	Task<dynamic> VerifySecurityCodeAsync(VerifySecurityCodeDto verifySecurityCodeDto);
	Task<dynamic> ChangeForgotPasswordAsync(ResetPasswordDto resetPasswordDto);
	Task<dynamic> ActiveAccountAsync(VerifySecurityCodeDto verifySecurityCodeDto);

}
