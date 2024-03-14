using AutoMapper;
using Castle.Core.Internal;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Security;

namespace EnrollmentManagementSoftware.Services.Implements;

public class RoleService : IRoleService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	public RoleService(DatabaseContext dbContext, IMapper mapper)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
	}

	public async Task<dynamic> AddPermissionAsync(int roleId, List<int> permissionsId)
	{
		try
		{
			var role = await dbContext.Roles.FindAsync(roleId);
			if(role == null) return new { status = false, message = "Id Does Not Exist" };
			foreach (var i in permissionsId)
			{
				if (await dbContext.Permissions.FindAsync(i) != null)
				{
						role.Permissions.Add(await dbContext.Permissions.FindAsync(i));
				}

			}
			dbContext.Entry(role).State = EntityState.Modified;
			if (await dbContext.SaveChangesAsync() > 0)
			{
				return new { status = true, message = "Ok" };
			}
			else
			{
				return new { status = false, message = "Failure" };
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.Roles.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			var role = await dbContext.Roles.FindAsync(id);
			role.Permissions!.Clear();
			dbContext.Roles.Remove(role);
			if (await dbContext.SaveChangesAsync() > 0)
			{
				return new { status = true, message = "Ok" };
			}
			else
			{
				return new { status = false, message = "Failure" };
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> DeletePermissionAsync(int roleId, List<int> permissionsId)
	{
		try
		{
			var role = await dbContext.Roles.FindAsync(roleId);
			if (role == null) return new { status = false, message = "Id Does Not Exist" };
			foreach (var i in permissionsId)
			{
				if (await dbContext.Permissions.FindAsync(i) != null)
				{
                    foreach (var j in role.Permissions)
                    {
                        if(i == j.Id)
						{
							role.Permissions.Remove(await dbContext.Permissions.FindAsync(i));
						}
                    }
                    
				}

			}
			dbContext.Entry(role).State = EntityState.Modified;
			if (await dbContext.SaveChangesAsync() > 0)
			{
				return new { status = true, message = "Ok" };
			}
			else
			{
				return new { status = false, message = "Failure" };
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetAsync(int id)
	{
		try
		{
			if (await dbContext.Roles.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var role = await dbContext.Roles.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				permissions = x.Permissions == null?  null : x.Permissions.Select(i => new
				{
					id = i.Id,
					name = i.Name
				}).ToList(),
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = role
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetByNameAsync(string name)
	{
		try
		{
			if (await dbContext.Roles.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var roles = await dbContext.Roles.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				permissions = x.Permissions == null ? null : x.Permissions.Select(i => new
				{
					id = i.Id,
					name = i.Name
				}).ToList(),
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = roles
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetListAsync()
	{
		try
		{
			if (await dbContext.Roles.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var roles = await dbContext.Roles.Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				permissions = x.Permissions == null ? null : x.Permissions.Select(i => new
				{
					id = i.Id,
					name = i.Name
				}).ToList(),
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = roles
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(RoleDto roleDto)
	{
		var role = mapper.Map<Role>(roleDto);
		try
		{
			if(await dbContext.Roles.FirstOrDefaultAsync(x=>x.Name.ToLower() == roleDto.Name.ToLower()) != null)
			{
				return new { status = true, message = "Name Already" };
			}
			role.CreatedDate = DateTime.Now;
			role.UpdatedDate = DateTime.Now;
			if (roleDto.PermissionsId.IsNullOrEmpty())
			{
				role.Permissions = new List<Permission>();
			}
			else
			{
				foreach(var i in roleDto.PermissionsId)
				{
					if(await dbContext.Permissions.FindAsync(i) != null)
					{
						role.Permissions.Add(await dbContext.Permissions.FindAsync(i));
					}
					
				}
			}
			await dbContext.Roles.AddAsync(role);
			if (await dbContext.SaveChangesAsync() > 0)
			{
				return new { status = true, message = "Ok" };
			}
			else
			{
				return new { status = false, message = "Failure" };
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> UpdateAsync(int id, RoleDto roleDto)
	{
		var role = mapper.Map<Role>(roleDto);
		try
		{
			if (await dbContext.Roles.AsNoTracking().FirstOrDefaultAsync(x => x.Name.ToLower() == role.Name.ToLower() && x.Id != id) != null)
			{
				return new { status = true, message = "Name Already" };
			}
			var roleModel = await dbContext.Roles.FindAsync(id);
			if(roleModel == null) return new { status = false, message = "Id Does Not Exist" };
			
			role.UpdatedDate = DateTime.Now;
			if (roleDto.PermissionsId.IsNullOrEmpty())
			{
				role.Permissions = new List<Permission>();
			}
			else
			{
				foreach (var i in roleDto.PermissionsId)
				{
					if (await dbContext.Permissions.FindAsync(i) != null)
					{
						role.Permissions.Add(await dbContext.Permissions.FindAsync(i));
					}

				}
			}
			roleModel.Name = role.Name;
			roleModel.UpdatedDate = role.UpdatedDate;
			roleModel.Permissions = role.Permissions;
			dbContext.Entry(roleModel).State = EntityState.Modified;
			if (await dbContext.SaveChangesAsync() > 0)
			{
				return new { status = true, message = "Ok" };
			}
			else
			{
				return new { status = false, message = "Failure" };
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}
}
