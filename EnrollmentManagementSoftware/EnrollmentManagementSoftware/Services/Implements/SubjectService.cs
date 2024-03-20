using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnrollmentManagementSoftware.Services.Implements;

public class SubjectService : ISubjectService
{

	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public SubjectService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.Subjects.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.Subjects.Remove(await dbContext.Subjects.FindAsync(id));
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

	
	public async Task<dynamic> GetByNameAsync(string name)
	{
		try
		{
			if (await dbContext.Subjects.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var subjectGroups = await dbContext.Subjects.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				code = x.Code,
				name = x.Name,
				course = new
				{
					id =x.Course.Id,
					name = x.Course.Name
				},
				subjectGroup = new
				{
					id = x.SubjectGroup.Id,
					name = x.SubjectGroup.Name
				},
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = subjectGroups
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
			if (await dbContext.Subjects.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var subjectGroups = await dbContext.Subjects.Select(x => new
			{
				id = x.Id,
				code = x.Code,
				name = x.Name,
				course = new
				{
					id = x.Course.Id,
					name = x.Course.Name
				},
				subjectGroup = new
				{
					id = x.SubjectGroup.Id,
					name = x.SubjectGroup.Name
				},
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = subjectGroups
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(SubjectDto subjectDto)
	{
		var subject = mapper.Map<Subject>(subjectDto);
		try
		{
			subject.CreatedDate = DateTime.Now;
			subject.UpdatedDate = DateTime.Now;
			subject.CreateBy = await dbContext.Users.
							FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString()));
			if (await dbContext.Courses.FindAsync(subjectDto.CourseId) == null)
			{
				return new { status = false, message = "Course Does Not Exist" };
			}
			subject.Course = await dbContext.Courses.FindAsync(subjectDto.CourseId);
			if(await dbContext.SubjectGroups.FindAsync(subjectDto.SubjectGroupId) == null)
			{
				return new { status = false, message = "Subject Group Does Not Exist" };
			}
			subject.SubjectGroup = await dbContext.SubjectGroups.FindAsync(subjectDto.SubjectGroupId);
			await dbContext.Subjects.AddAsync(subject);
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

	public async Task<dynamic> UpdateAsync(int id, SubjectDto subjectDto)
	{
		var subject = mapper.Map<Subject>(subjectDto);
		try
		{
			var subjectModel = await dbContext.Subjects.FindAsync(id);
			if (subjectModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			subjectModel.Name = subject.Name;
			subjectModel.Code = subject.Code;
			subjectModel.UpdatedDate = DateTime.Now;

			if (await dbContext.Courses.FindAsync(subjectDto.CourseId) == null)
			{
				return new { status = false, message = "Course Does Not Exist" };
			}
			subjectModel.Course = await dbContext.Courses.FindAsync(subjectDto.CourseId);
			if (await dbContext.SubjectGroups.FindAsync(subjectDto.SubjectGroupId) == null)
			{
				return new { status = false, message = "Subject Group Does Not Exist" };
			}
			subjectModel.SubjectGroup = await dbContext.SubjectGroups.FindAsync(subjectDto.SubjectGroupId);
			dbContext.Entry(subjectModel).State = EntityState.Modified;
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
			if (await dbContext.Subjects.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var subjects = await dbContext.Subjects.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				code = x.Code,
				name = x.Name,
				course = new
				{
					id = x.Course.Id,
					name = x.Course.Name
				},
				subjectGroup = new
				{
					id = x.SubjectGroup.Id,
					name = x.SubjectGroup.Name
				},
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = subjects
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

}
