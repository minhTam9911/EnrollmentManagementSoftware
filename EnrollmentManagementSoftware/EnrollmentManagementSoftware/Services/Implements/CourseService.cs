using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentManagementSoftware.Services.Implements;

public class CourseService : ICourseService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	private IConfiguration configuration;
	public CourseService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
		this.configuration = configuration;
	}
	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.Courses.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.Courses.Remove(await dbContext.Courses.FindAsync(id));
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

	public async Task<dynamic> GetDetailAsync(int id)
	{
		try
		{
			if (await dbContext.Courses.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var course = await dbContext.Courses.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				subjects = x.Subjects == null ? null : x.Subjects.Select(i => new
				{
					id = i.Id,
					name = i.Name,
					subjectGroup = i.SubjectGroup == null ? null : new
					{
						id = i.SubjectGroup.Id,
						name = i.SubjectGroup.Name
					}
				}),
				classrooms = x.Classrooms == null ? null : x.Classrooms.Select(j => new
				{
					id = j.Id,
					code = j.Code,
					name = j.Name,
					image = configuration["BaseUrl"]+"classrooms/"+j.Image,
					description = j.Description
					
				}),
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = course
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
			if (await dbContext.Courses.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var courses = await dbContext.Courses.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = courses
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
			if (await dbContext.Courses.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var courses = await dbContext.Courses.Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = courses
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(CourseDto courseDto)
	{
		var course = mapper.Map<Course>(courseDto);
		try
		{
			course.CreatedDate = DateTime.Now;
			course.UpdatedDate = DateTime.Now;
			//tuitionType.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			await dbContext.Courses.AddAsync(course);
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

	public async Task<dynamic> UpdateAsync(int id, CourseDto courseDto)
	{
		var course = mapper.Map<Course>(courseDto);
		try
		{
			var courseModel = await dbContext.Courses.FindAsync(id);
			if (courseModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			course.UpdatedDate = DateTime.Now;
			courseModel.Name = course.Name;
			courseModel.UpdatedDate = courseModel.UpdatedDate;
			dbContext.Entry(courseModel).State = EntityState.Modified;
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
			if (await dbContext.Courses.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var course = await dbContext.Courses.Where(x => x.Id == id).Select(x => new
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
				data = course
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}
}
