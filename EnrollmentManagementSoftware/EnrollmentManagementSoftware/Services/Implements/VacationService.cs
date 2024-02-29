using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentManagementSoftware.Services.Implements;

public class VacationService : IVacationService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public VacationService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}
	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.Vacations.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.Vacations.Remove(await dbContext.Vacations.FindAsync(id));
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
			if (await dbContext.Vacations.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var vacation = await dbContext.Vacations.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				reason = x.Reason,
				startDate = x.StartDate,
				endDate = x.EndDate,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = vacation
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
			if (await dbContext.Vacations.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var vacations = await dbContext.Vacations.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				reason = x.Reason,
				startDate = x.StartDate,
				endDate = x.EndDate,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = vacations
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
			if (await dbContext.Vacations.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var vacations = await dbContext.Vacations.Select(x => new
			{
				id = x.Id,
				name = x.Name,
				reason = x.Reason,
				startDate = x.StartDate,
				endDate = x.EndDate,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = vacations
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(VacationDto vacationDto)
	{
		var vacation = mapper.Map<Vacation>(vacationDto);
		try
		{
			vacation.CreatedDate = DateTime.Now;
			vacation.UpdatedDate = DateTime.Now;
			//tuitionType.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			await dbContext.Vacations.AddAsync(vacation);
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

	public async Task<dynamic> UpdateAsync(int id, VacationDto vacationDto)
	{
		var vacation = mapper.Map<Vacation>(vacationDto);
		try
		{
			var vacationModel = await dbContext.Vacations.FindAsync(id);
			if (vacationModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			vacation.UpdatedDate = DateTime.Now;
			vacationModel.Name = vacation.Name;
			vacationModel.Reason = vacation.Reason;
			vacationModel.UpdatedDate = vacation.UpdatedDate;
			dbContext.Entry(vacationModel).State = EntityState.Modified;
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
