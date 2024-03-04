using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentManagementSoftware.Services.Implements;

public class GradeTypeService : IGrandeTypeService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public GradeTypeService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.GradeTypes.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.GradeTypes.Remove(await dbContext.GradeTypes.FindAsync(id));
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
			if (await dbContext.GradeTypes.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var gradeType = await dbContext.GradeTypes.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				academicYear = new
				{
					id = x.AcademicYear.Id,
					name = x.AcademicYear.Name
				},
				subject = new
				{
					id = x.Subject.Id,
					name = x.Subject.Name
				},
				gradingMethod = new
				{
					id = x.GradingMethod.Id,
					name = x.GradingMethod.Name
				},
				numberOfColumn = x.NumberOfColumn,
				numberOfRequiredColumn = x.NumberOfRequiredColumn,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = gradeType
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
			if (await dbContext.GradeTypes.Where(x => x.AcademicYear.Name!.ToLower().Contains(name.ToLower()) || x.Subject.Name!.ToLower().Contains(name.ToLower()) || x.GradingMethod.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var gradeTypes = await dbContext.GradeTypes.Where(x => x.AcademicYear.Name!.ToLower().Contains(name.ToLower()) || x.Subject.Name!.ToLower().Contains(name.ToLower()) || x.GradingMethod.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				academicYear = new
				{
					id = x.AcademicYear.Id,
					name = x.AcademicYear.Name
				},
				subject = new
				{
					id = x.Subject.Id,
					name = x.Subject.Name
				},
				gradingMethod = new
				{
					id = x.GradingMethod.Id,
					name = x.GradingMethod.Name
				},
				numberOfColumn = x.NumberOfColumn,
				numberOfRequiredColumn = x.NumberOfRequiredColumn,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = gradeTypes
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
			if (await dbContext.GradeTypes.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var gradeTypes = await dbContext.GradeTypes.Select(x => new
			{
				id = x.Id,
				academicYear = new
				{
					id = x.AcademicYear.Id,
					name = x.AcademicYear.Name
				},
				subject = new
				{
					id = x.Subject.Id,
					name = x.Subject.Name
				},
				gradingMethod = new
				{
					id = x.GradingMethod.Id,
					name = x.GradingMethod.Name
				},
				numberOfColumn = x.NumberOfColumn,
				numberOfRequiredColumn = x.NumberOfRequiredColumn,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = gradeTypes
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(GradeTypeDto gradeTypeDto)
	{
		var gradeType = mapper.Map<GradeType>(gradeTypeDto);
		try
		{
			gradeType.CreatedDate = DateTime.Now;
			gradeType.UpdatedDate = DateTime.Now;
			if((await dbContext.AcademicYears.FindAsync(gradeTypeDto.AcademicYearId)) == null)
			{
				return new { status = false, message = "Academic Year Does Not Exist" };
			}
			gradeType.AcademicYear = await dbContext.AcademicYears.FindAsync(gradeTypeDto.AcademicYearId);
			if ((await dbContext.Subjects.FindAsync(gradeTypeDto.SubjectId)) == null)
			{
				return new { status = false, message = "Subject Does Not Exist" };
			}
			gradeType.Subject = await dbContext.Subjects.FindAsync(gradeTypeDto.SubjectId);
			if ((await dbContext.GradingMethods.FindAsync(gradeTypeDto.GradingMethodId)) == null)
			{
				return new { status = false, message = "Grading Method Does Not Exist" };
			}
			gradeType.GradingMethod = await dbContext.GradingMethods.FindAsync(gradeTypeDto.GradingMethodId);
			//tuitionType.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			await dbContext.GradeTypes.AddAsync(gradeType);
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

	public async Task<dynamic> UpdateAsync(int id, GradeTypeDto gradeTypeDto)
	{
		var gradeType = mapper.Map<GradeType>(gradeTypeDto);
		try
		{
			var gradeTypeModel = await dbContext.GradeTypes.FindAsync(id);
			if (gradeTypeModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			gradeTypeModel.UpdatedDate = DateTime.Now;
			if ((await dbContext.AcademicYears.FindAsync(gradeTypeDto.AcademicYearId)) == null)
			{
				return new { status = false, message = "Academic Year Does Not Exist" };
			}
			gradeTypeModel.AcademicYear = await dbContext.AcademicYears.FindAsync(gradeTypeDto.AcademicYearId);
			if ((await dbContext.Subjects.FindAsync(gradeTypeDto.SubjectId)) == null)
			{
				return new { status = false, message = "Subject Does Not Exist" };
			}
			gradeTypeModel.Subject = await dbContext.Subjects.FindAsync(gradeTypeDto.SubjectId);
			if ((await dbContext.GradingMethods.FindAsync(gradeTypeDto.GradingMethodId)) == null)
			{
				return new { status = false, message = "Grading Method Does Not Exist" };
			}
			gradeTypeModel.GradingMethod = await dbContext.GradingMethods.FindAsync(gradeTypeDto.GradingMethodId);
			dbContext.Entry(gradeTypeModel).State = EntityState.Modified;
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
