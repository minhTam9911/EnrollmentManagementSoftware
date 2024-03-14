using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace EnrollmentManagementSoftware.Services.Implements;

public class PermissionService : IPermissionService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	public PermissionService(DatabaseContext dbContext, IMapper mapper)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if(await dbContext.Permissions.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.Permissions.Remove(await dbContext.Permissions.FindAsync(id));
			if(await dbContext.SaveChangesAsync() > 0)
			{
				return new { status = true, message = "Ok" };
			}
			else
			{
				return new { status = false, message = "Failure" };
			}
		}catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetAsync(int id)
	{
		try
		{
			if (await dbContext.Permissions.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var permission  = await dbContext.Permissions.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = permission
			};
		}
		catch(Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetByNameAsync(string name)
	{
		try
		{
			if (await dbContext.Permissions.Where(x=>x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var permissions = await dbContext.Permissions.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = permissions
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
			if (await dbContext.Permissions.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var permissions = await dbContext.Permissions.Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = permissions
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(PermissionDto permissionDto)
	{
		var permission = mapper.Map<Permission>(permissionDto);
		try
		{
			if (await dbContext.Permissions.FirstOrDefaultAsync(x => x.Name.ToLower() == permission.Name.ToLower()) != null)
			{
				return new { status = true, message = "Name Already" };
			}
			permission.CreatedDate = DateTime.Now;
			permission.UpdatedDate = DateTime.Now;
			await dbContext.Permissions.AddAsync(permission);
			if(await dbContext.SaveChangesAsync() > 0)
			{
				return new { status = true, message = "Ok" };
			}
			else
			{
				return new { status = false, message = "Failure" };
			}
		}catch(Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> UpdateAsync(int id, PermissionDto permissionDto)
	{
		var permission = mapper.Map<Permission>(permissionDto);
		try
		{
			if (await dbContext.Permissions.AsNoTracking().FirstOrDefaultAsync(x => x.Name.ToLower() == permission.Name.ToLower()&& x.Id!=id) != null)
			{
				return new { status = true, message = "Name Already" };
			}
			var permissionModel = await dbContext.Permissions.FindAsync(id);
			if (permissionModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			permission.UpdatedDate = DateTime.Now;
			permissionModel.Name = permission.Name;
			permissionModel.UpdatedDate = permission.UpdatedDate;
			dbContext.Entry(permissionModel).State = EntityState.Modified;
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
