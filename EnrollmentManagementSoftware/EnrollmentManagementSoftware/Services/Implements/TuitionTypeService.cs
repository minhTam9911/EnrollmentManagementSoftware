using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnrollmentManagementSoftware.Services.Implements;

public class TuitionTypeService : ITuitionTypeService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public TuitionTypeService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}
	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.TuitionTypes.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.TuitionTypes.Remove(await dbContext.TuitionTypes.FindAsync(id));
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
			if (await dbContext.TuitionTypes.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var tuitionType = await dbContext.TuitionTypes.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = tuitionType
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
			if (await dbContext.TuitionTypes.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var tuitionTypes = await dbContext.TuitionTypes.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = tuitionTypes
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
			if (await dbContext.TuitionTypes.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var tuitionTypes = await dbContext.TuitionTypes.Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = tuitionTypes
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(TuitionTypeDto tuitionTypeDto)
	{
		var tuitionType = mapper.Map<TuitionType>(tuitionTypeDto);
		try
		{
			tuitionType.CreatedDate = DateTime.Now;
			tuitionType.UpdatedDate = DateTime.Now;
			tuitionType.CreateBy = await dbContext.Users.
							FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString()));
			await dbContext.TuitionTypes.AddAsync(tuitionType);
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

	public async Task<dynamic> UpdateAsync(int id, TuitionTypeDto tuitionTypeDto)
	{
		var tuitionType = mapper.Map<TuitionType>(tuitionTypeDto);
		try
		{
			var tuitionTypeModel = await dbContext.TuitionTypes.FindAsync(id);
			if (tuitionTypeModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			tuitionType.UpdatedDate = DateTime.Now;
			tuitionTypeModel.Name = tuitionType.Name;
			tuitionTypeModel.UpdatedDate = tuitionType.UpdatedDate;
			dbContext.Entry(tuitionTypeModel).State = EntityState.Modified;
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
