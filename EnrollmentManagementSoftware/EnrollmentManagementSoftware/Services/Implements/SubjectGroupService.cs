using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnrollmentManagementSoftware.Services.Implements;

public class SubjectGroupService : ISubjectGroupService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public SubjectGroupService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.SubjectGroups.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.SubjectGroups.Remove(await dbContext.SubjectGroups.FindAsync(id));
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
			if (await dbContext.SubjectGroups.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var subjectGroup = await dbContext.SubjectGroups.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				subjects = x.Subjects == null? null: x.Subjects.Select(i=>new
				{
					id = i.Id,
					name = i.Name,
					course = i.Course == null? null: new
					{
						id = i.Course.Id,
						name = i.Course.Name
					}
				}),
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = subjectGroup
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
			if (await dbContext.SubjectGroups.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var subjectGroups = await dbContext.SubjectGroups.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
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
			if (await dbContext.SubjectGroups.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var subjectGroups = await dbContext.SubjectGroups.Select(x => new
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
				data = subjectGroups
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(SubjectGroupDto subjectGroupDto)
	{
		var subjectGroup = mapper.Map<SubjectGroup>(subjectGroupDto);
		try
		{
			subjectGroup.CreatedDate = DateTime.Now;
			subjectGroup.UpdatedDate = DateTime.Now;
			subjectGroup.CreateBy = await dbContext.Users.
							FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString()));
			await dbContext.SubjectGroups.AddAsync(subjectGroup);
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

	public async Task<dynamic> UpdateAsync(int id, SubjectGroupDto subjectGroupDto)
	{
		var subjectGroup = mapper.Map<SubjectGroup>(subjectGroupDto);
		try
		{
			var subjectGroupModel = await dbContext.SubjectGroups.FindAsync(id);
			if (subjectGroupModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			subjectGroup.UpdatedDate = DateTime.Now;
			subjectGroupModel.Name = subjectGroup.Name;
			subjectGroupModel.UpdatedDate = subjectGroup.UpdatedDate;
			dbContext.Entry(subjectGroupModel).State = EntityState.Modified;
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
			if (await dbContext.SubjectGroups.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var subjectGroup = await dbContext.SubjectGroups.Where(x => x.Id == id).Select(x => new
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
				data = subjectGroup
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}
}
