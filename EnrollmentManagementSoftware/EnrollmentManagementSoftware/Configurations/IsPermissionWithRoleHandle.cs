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
		if (role == "Admin" || role == "Manager")
		{
			context.Succeed(requirement);
		}
		else
		{
			var permissionRole = dbContext.Roles.FirstOrDefault(x => x.Name == role);
			if (permissionRole == null)
			{
				context.Fail();
			}
			else
			{
				foreach (var i in permissionRole.Permissions)
				{
					if (i.Name.ToLowerInvariant() == requirement.Permission.ToLowerInvariant())
					{
						context.Succeed(requirement);
					}
				}
			}
			
			
		}
		return Task.CompletedTask;
	}
}
