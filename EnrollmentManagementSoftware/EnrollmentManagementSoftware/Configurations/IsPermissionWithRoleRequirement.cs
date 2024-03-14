using Microsoft.AspNetCore.Authorization;

namespace EnrollmentManagementSoftware.Configurations;

public class IsPermissionWithRoleRequirement : IAuthorizationRequirement
{
	public string Permission { get; set; }
	public IsPermissionWithRoleRequirement(string permission)
	{
		Permission = permission;
	}
}
