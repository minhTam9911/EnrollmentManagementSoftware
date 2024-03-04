using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentManagementSoftware.Services.Implements;

public class GradeService : IGradeService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public GradeService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.Grades.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.Grades.Remove(await dbContext.Grades.FindAsync(id));
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
			if (await dbContext.Grades.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var grade = await dbContext.Grades.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				subject = new
				{
					id = x.Subject.Id,
					name = x.Subject.Name
				},
				gradeType = new
				{
					id = x.GradeType.Id,
					numberOfColumn = x.GradeType.NumberOfColumn,
					numberOfRequiredColumn = x.GradeType.NumberOfRequiredColumn,
				},
				student = new
				{
					firstName = x.Student.FirstName,
					lastName = x.Student.LastName
				},
				points = x.Points == null ? null : x.Points.Select(i => new
				{
					index = i.Id,
					score = i.Score
				}),
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = grade
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
			if (await dbContext.Grades.Where(x => x.Student.FirstName!.ToLower().Contains(name.ToLower()) || x.Subject.Name!.ToLower().Contains(name.ToLower()) || x.Student.LastName!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var grades = await dbContext.Grades.Where(x => x.Student.FirstName!.ToLower().Contains(name.ToLower()) || x.Subject.Name!.ToLower().Contains(name.ToLower()) || x.Student.LastName!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				subject = new
				{
					id = x.Subject.Id,
					name = x.Subject.Name
				},
				gradeType = new
				{
					id = x.GradeType.Id,
					numberOfColumn = x.GradeType.NumberOfColumn,
					numberOfRequiredColumn = x.GradeType.NumberOfRequiredColumn,
				},
				student = new
				{
					firstName = x.Student.FirstName,
					lastName = x.Student.LastName
				},
				points = x.Points == null ? null : x.Points.Select(i => new
				{
					index = i.Id,
					score = i.Score
				}),
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = grades
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
			if (await dbContext.Grades.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var grades = await dbContext.Grades.Select(x => new
			{
				id = x.Id,
				subject = new
				{
					id = x.Subject.Id,
					name = x.Subject.Name
				},
				gradeType = new
				{
					id = x.GradeType.Id,
					numberOfColumn = x.GradeType.NumberOfColumn,
					numberOfRequiredColumn = x.GradeType.NumberOfRequiredColumn,
				},
				student = new
				{
					firstName = x.Student.FirstName,
					lastName = x.Student.LastName
				},
				points = x.Points == null ? null : x.Points.Select(i => new
				{
					index = i.Id,
					score = i.Score
				}),
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = grades
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(GradeDto gradeDto)
	{
		var grade = mapper.Map<Grade>(gradeDto);
		try
		{
			grade.CreatedDate = DateTime.Now;
			grade.UpdatedDate = DateTime.Now;
			if ((await dbContext.Students.FindAsync(gradeDto.StudentId)) == null)
			{
				return new { status = false, message = "Student Does Not Exist" };
			}
			grade.Student = await dbContext.Students.FindAsync(gradeDto.StudentId);
			if ((await dbContext.Subjects.FindAsync(gradeDto.SubjectId)) == null)
			{
				return new { status = false, message = "Subject Does Not Exist" };
			}
			grade.Subject = await dbContext.Subjects.FindAsync(gradeDto.SubjectId);
			if ((await dbContext.GradeTypes.FindAsync(gradeDto.GradeTypeId)) == null)
			{
				return new { status = false, message = "Grading Type Does Not Exist" };
			}
			grade.GradeType = await dbContext.GradeTypes.FindAsync(grade.GradeType);
			//tuitionType.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			if ((await dbContext.Grades.Where(x => x.Student.Id == gradeDto.StudentId && x.Subject.Id == gradeDto.SubjectId).AnyAsync()))
			{
				return new { status = false, message = "This Student Has This Subject Already Exists" };
			}
			if(grade.GradeType.NumberOfColumn < gradeDto.Points.Count)
			{
				return new { status = false, message = "The total number of score columns that have exceeded the specified limit is "+ grade.GradeType.NumberOfColumn};
			}
			foreach(var i in gradeDto.Points)
			{
				grade.Points.Add(new Point { Score = i});
			}
			await dbContext.Grades.AddAsync(grade);
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

	public async Task<dynamic> UpdateAsync(int id, GradeDto gradeDto)
	{
		var grade = mapper.Map<Grade>(gradeDto);
		try
		{
			var gradeModel = await dbContext.Grades.FindAsync(id);
			if (gradeModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			gradeModel.UpdatedDate = DateTime.Now;
			if (grade.GradeType.NumberOfColumn < gradeDto.Points.Count)
			{
				return new { status = false, message = "The total number of score columns that have exceeded the specified limit is " + grade.GradeType.NumberOfColumn };
			}
			gradeModel.Points.Clear();
			foreach (var i in gradeDto.Points)
			{
				grade.Points.Add(new Point { Score = i });
			}
			dbContext.Entry(gradeModel).State = EntityState.Modified;
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
