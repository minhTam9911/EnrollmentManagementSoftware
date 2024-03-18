using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnrollmentManagementSoftware.Services.Implements;

public class AcademicYearService : IAcademicYearService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public AcademicYearService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.AcademicYears.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.AcademicYears.Remove(await dbContext.AcademicYears.FindAsync(id));
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
			if (await dbContext.AcademicYears.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var academicYear = await dbContext.AcademicYears.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				code = x.Code,
				name = x.Name,
				startDate = x.StartDate,
				endDate = x.EndDate,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = academicYear
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
			if (await dbContext.AcademicYears.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var academicYear = await dbContext.AcademicYears.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				code = x.Code,
				name = x.Name,
				startDate = x.StartDate,
				endDate = x.EndDate,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = academicYear
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
			if (await dbContext.AcademicYears.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var academicYears = await dbContext.AcademicYears.Select(x => new
			{
				id = x.Id,
				code = x.Code,
				name = x.Name,
				startDate = x.StartDate,
				endDate = x.EndDate,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = academicYears
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(AcademicYearDto academicYearDto)
	{
		var academicYear = mapper.Map<AcademicYear>(academicYearDto);
		try
		{
			academicYear.CreatedDate = DateTime.Now;
			academicYear.UpdatedDate = DateTime.Now;
			academicYear.CreateBy = await dbContext.Users.
							FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString()));
			await dbContext.AcademicYears.AddAsync(academicYear);
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

	public async Task<dynamic> UpdateAsync(int id, AcademicYearDto academicYearDto)
	{
		var academicYear = mapper.Map<AcademicYear>(academicYearDto);
		try
		{
			var academicYearModel = await dbContext.AcademicYears.FindAsync(id);
			if (academicYearModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			academicYearModel.UpdatedDate = DateTime.Now;
			academicYearModel.Code = academicYear.Code;
			academicYearModel.Name = academicYear.Name;
			academicYearModel.StartDate = academicYear.StartDate;
			academicYearModel.EndDate = academicYear.EndDate;
			academicYearModel.UpdatedDate = academicYear.UpdatedDate;

			dbContext.Entry(academicYearModel).State = EntityState.Modified;
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
