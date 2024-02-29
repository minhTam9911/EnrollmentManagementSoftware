using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentManagementSoftware.Services.Implements;

public class GradingMethodService : IGrandingMethodService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public GradingMethodService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}
	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.GradingMethods.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.GradingMethods.Remove(await dbContext.GradingMethods.FindAsync(id));
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
			if (await dbContext.GradingMethods.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var gradingMethod = await dbContext.GradingMethods.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				multiplierFactor = x.MultiplierFactor,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = gradingMethod
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
			if (await dbContext.GradingMethods.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var gradingMethods = await dbContext.GradingMethods.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				multiplierFactor = x.MultiplierFactor,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = gradingMethods
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
			if (await dbContext.GradingMethods.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var grandingMethods = await dbContext.GradingMethods.Select(x => new
			{
				id = x.Id,
				name = x.Name,
				multiplierFactor = x.MultiplierFactor,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = grandingMethods
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(GradingMethodDto gradingMethodDto)
	{
		var gradingMethod = mapper.Map<GradingMethod>(gradingMethodDto);
		try
		{
			gradingMethod.CreatedDate = DateTime.Now;
			gradingMethod.UpdatedDate = DateTime.Now;
			//tuitionType.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			await dbContext.GradingMethods.AddAsync(gradingMethod);
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

	public async Task<dynamic> UpdateAsync(int id, GradingMethodDto gradingMethodDto)
	{
		var gradingMethod = mapper.Map<GradingMethod>(gradingMethodDto);
		try
		{
			var gradingMethodModel = await dbContext.GradingMethods.FindAsync(id);
			if (gradingMethodModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			gradingMethod.UpdatedDate = DateTime.Now;
			gradingMethodModel.Name = gradingMethod.Name;
			gradingMethodModel.MultiplierFactor = gradingMethod.MultiplierFactor;
			gradingMethodModel.UpdatedDate = gradingMethod.UpdatedDate;
			dbContext.Entry(gradingMethodModel).State = EntityState.Modified;
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
