using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net;
using System.Security.Claims;

namespace EnrollmentManagementSoftware.Configurations;

public class IsPermissionWithRoleHandle : AuthorizationHandler<IsPermissionWithRoleRequirement>
{
	private readonly DatabaseContext dbContext;
	public IsPermissionWithRoleHandle(DatabaseContext dbContext)
	{
		this.dbContext = dbContext;
	}

	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsPermissionWithRoleRequirement requirement)
	{
		var role = context.User.FindFirstValue(ClaimTypes.Role);
		if (role == "Admin")
		{
			context.Succeed(requirement);
		}
		else
		{
			var permissionRole = dbContext.Roles.FirstOrDefault(x => x.Name == role);
			foreach (var i in permissionRole.Permissions)
			{
				if (requirement.Permission == i.Name )
				{
					context.Succeed(requirement);
				}
			}
			context.Fail();
		}
		return Task.CompletedTask;
	}
}
