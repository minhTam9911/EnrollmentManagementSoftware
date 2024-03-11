using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Helpers;
using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace EnrollmentManagementSoftware.Services.Implements;

public class ClassroomService : IClassroomService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	private IWebHostEnvironment webHostEnvironment;
	public ClassroomService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
		this.webHostEnvironment = webHostEnvironment;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.Classrooms.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.Classrooms.Remove(await dbContext.Classrooms.FindAsync(id));
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
			if (await dbContext.Classrooms.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var permission = await dbContext.Classrooms.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				course = new
				{
					id = x.Course.Id,
					name = x.Course.Name
				},
				academicYear = new
				{
					id = x.AcademicYear.Id,
					code = x.AcademicYear.Code,
					name = x.AcademicYear.Name
				},
				capacity = x.Capacity,
				tuitionFee = x.TuitionFee,
				description = x.Description,
				image = x.Image,
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
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetByNameAsync(string name)
	{
		try
		{
			if (await dbContext.Classrooms.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var classrooms = await dbContext.Classrooms.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				course = new
				{
					id = x.Course.Id,
					name = x.Course.Name
				},
				academicYear = new
				{
					id = x.AcademicYear.Id,
					code = x.AcademicYear.Code,
					name = x.AcademicYear.Name
				},
				capacity = x.Capacity,
				tuitionFee = x.TuitionFee,
				description = x.Description,
				image = x.Image,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = classrooms
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
			if (await dbContext.Classrooms.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var classrooms = await dbContext.Classrooms.Select(x => new
			{
				id = x.Id,
				name = x.Name,
				course = new
				{
					id = x.Course.Id,
					name = x.Course.Name
				},
				academicYear = new
				{
					id = x.AcademicYear.Id,
					code = x.AcademicYear.Code,
					name = x.AcademicYear.Name
				},
				capacity = x.Capacity,
				tuitionFee = x.TuitionFee,
				description = x.Description,
				image = x.Image,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = classrooms
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(ClassroomDto classroomDto)
	{
		var classroom = mapper.Map<Classroom>(classroomDto);
		try
		{
			if (await dbContext.AcademicYears.FindAsync(classroomDto.AcademicYearId) == null)
			{
				return new { status = false, message = "Academic Year Does Not Exist" };
			}
			classroom.AcademicYear = await dbContext.AcademicYears.FindAsync(classroomDto.AcademicYearId);
			if (await dbContext.Courses.FindAsync(classroomDto.CourseId) == null)
			{
				return new { status = false, message = "Course Does Not Exist" };
			}
			classroom.Course = await dbContext.Courses.FindAsync(classroomDto.CourseId);
			//tuitionType.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));



			if (!FileHelper.IsImage(classroomDto.Image))
			{
				return new { status = false, message = "The attached file is not an image file" };
			}
			var fileName = FileHelper.GenerateFileName(classroomDto.Image.FileName);
			var path = Path.Combine(webHostEnvironment.WebRootPath, "classrooms", fileName);
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				classroomDto.Image.CopyTo(fileStream);
			}
			classroom.Image = fileName;
			classroom.Status = false;
			await dbContext.Classrooms.AddAsync(classroom);
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

	public async Task<dynamic> UpdateAsync(int id, ClassroomDto classroomDto)
	{
		var classroom = mapper.Map<Classroom>(classroomDto);
		try
		{
			var classroomModel = await dbContext.Classrooms.FindAsync(id);
			if (classroomModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			classroom.UpdatedDate = DateTime.Now;
			classroomModel.Name = classroom.Name;
			classroomModel.Status = classroom.Status;
			classroomModel.Description = classroom.Description;
			classroomModel.TuitionFee = classroom.TuitionFee;
			classroomModel.Capacity = classroom.Capacity;
			if (await dbContext.AcademicYears.AsNoTracking().FirstOrDefaultAsync(x => x.Id == classroomDto.AcademicYearId) == null)
			{
				return new { status = false, message = "Academic Year Does Not Exist" };
			}
			classroomModel.AcademicYear = await dbContext.AcademicYears.FindAsync(classroomDto.AcademicYearId);
			if (await dbContext.Courses.AsNoTracking().FirstOrDefaultAsync(x => x.Id == classroomDto.CourseId) == null)
			{
				return new { status = false, message = "Course Does Not Exist" };
			}
			classroomModel.Course = await dbContext.Courses.FindAsync(classroomDto.CourseId);
			classroomModel.Name = classroom.Name;
			if (!FileHelper.IsImage(classroomDto.Image))
			{
				return new { status = false, message = "The attached file is not an image file" };
			}
			if (classroomModel.Image != classroomDto.Image.FileName)
			{
				var pathDelete = Path.Combine(webHostEnvironment.WebRootPath, "classrooms", classroomModel.Image);
				File.Delete(pathDelete);
				
				var fileName = FileHelper.GenerateFileName(classroomDto.Image.FileName);
				var path = Path.Combine(webHostEnvironment.WebRootPath, "classrooms", fileName);
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					classroomDto.Image.CopyTo(fileStream);
				}
				classroom.Image = fileName;
			}
			classroomModel.Image = classroom.Image;
			classroomModel.UpdatedDate = classroom.UpdatedDate;
			dbContext.Entry(classroomModel).State = EntityState.Modified;
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
