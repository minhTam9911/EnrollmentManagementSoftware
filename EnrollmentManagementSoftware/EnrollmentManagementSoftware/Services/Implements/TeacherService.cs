using AutoMapper;
using Castle.Core.Internal;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Helpers;
using EnrollmentManagementSoftware.Helplers;
using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace EnrollmentManagementSoftware.Services.Implements;

public class TeacherService : ITeacherService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IConfiguration configuration;
	private IWebHostEnvironment webHostEnvironment;
	private IHttpContextAccessor httpContextAccessor;
	public TeacherService(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.webHostEnvironment = webHostEnvironment;
		this.mapper = mapper;
		this.configuration = configuration;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<dynamic> DeleteAsync(Guid id)
	{
		try
		{
			if (await dbContext.Teachers.FindAsync(id) == null)
			{
				return new { status = false, message = "Id does not exist" };
			}
			else
			{
				dbContext.Teachers.Remove(await dbContext.Teachers!.FindAsync(id));
				if (await dbContext.SaveChangesAsync() > 0)
				{
					return new { status = true, message = "Ok" };
				}
				else
				{
					return new { status = false, message = "Failure" };
				}
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}
	 
	public async Task<dynamic> GetAsync(Guid id)
	{
		try
		{
			if (await dbContext.Teachers.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				var teacher = await dbContext.Teachers
				.Where(x =>x.Id == id)
					.Select(x => new {
						id = x.Id,
						code = x.Code,
						firstName = x.FirstName,
						lastName = x.LastName,
						address = x.Address,
						phoneNumber = x.PhoneNumber,
						email = x.Email,
						taxCode = x.TaxCode,
						gender = x.Gender,
						wage = x.Wage,
						image = x.Image,
						dateOfBirth = DateOnly.FromDateTime((DateTime)x.DayOfBirth!),
						majorSubject = new
						{
							id = x.MajorSubject.Id,
							code = x.MajorSubject.Code,
							name = x.MajorSubject.Name
						},
						minorSubject = new
						{
							id = x.MinorSubject.Id,
							code = x.MinorSubject.Code,
							name = x.MinorSubject.Name
						}
					})
					.FirstOrDefaultAsync();
				return new
				{
					status = true,
					message = "Ok",
					data = teacher
				};
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
			name = name.ToLower();

			if (await dbContext.Teachers.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				var teachers = await dbContext.Teachers
					.Where(x =>
							x.Code!.ToLower().Contains(name) ||
							x.LastName!.ToLower().Contains(name) ||
							x.Email!.ToLower().Contains(name) ||
							x.PhoneNumber!.Contains(name)
							)
					.Select(x => new {
						id = x.Id,
						code = x.Code,
						firstName = x.FirstName,
						lastName = x.LastName,
						address = x.Address,
						phoneNumber = x.PhoneNumber,
						email = x.Email,
						taxCode = x.TaxCode,
						gender = x.Gender,
						wage = x.Wage,
						image = x.Image,
						dateOfBirth = DateOnly.FromDateTime((DateTime)x.DayOfBirth!),
						majorSubject = new
						{
							id = x.MajorSubject.Id,
							code = x.MajorSubject.Code,
							name = x.MajorSubject.Name
						},
						minorSubject = new
						{
							id = x.MinorSubject.Id,
							code = x.MinorSubject.Code,
							name = x.MinorSubject.Name
						}
					})
					.ToListAsync();
				return new
				{
					status = true,
					message = "Ok",
					data = teachers
				};
			}
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
			if (await dbContext.Teachers.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
//				_ = keyword.IsNullOrEmpty() ? keyword = null : keyword!.ToLower();
//				var pageResults = 10f;
//				var pageCount = Math.Ceiling(await dbContext.Teachers.CountAsync() / pageResults);
				var teachers = await dbContext.Teachers
//					.Where(x =>
//							x.Code!.ToLower().Contains(keyword) ||
//							x.LastName!.ToLower().Contains(keyword) ||
//							x.Email!.ToLower().Contains(keyword) ||
//							x.PhoneNumber!.Contains(keyword)
//							)
//					.Skip((page - 1) * (int)pageResults)
//					.Take((int)pageResults)
					.Select(x => new {
						id = x.Id,
						code = x.Code,
						firstName = x.FirstName,
						lastName = x.LastName,
						address = x.Address,
						phoneNumber = x.PhoneNumber,
						email = x.Email,
						taxCode = x.TaxCode,
						gender = x.Gender,
						wage = x.Wage,
						image = x.Image,
						dateOfBirth = DateOnly.FromDateTime((DateTime)x.DayOfBirth!),
						majorSubject = new
						{
							id = x.MajorSubject.Id,
							code = x.MajorSubject.Code,
							name = x.MajorSubject.Name
						},
						minorSubject = new
						{
							id = x.MinorSubject.Id,
							code = x.MinorSubject.Code,
							name = x.MinorSubject.Name
						}
					})
					.ToListAsync();
				return new
				{

//					pages = (int)pageCount,
//					currentPage = page,
					status = true,
					message = "Ok",
					data = teachers
				};
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	
	public async Task<dynamic> InsertAsync(TeacherDto teacherDto)
	{
		var teacher = mapper.Map<Teacher>(teacherDto);
		var user = new User();
		try
		{
			if (teacherDto.Image == null)
			{
				teacher.Image = "avatar-default-icon.png";
			}
			else
			{
					if (FileHelper.IsImage(teacherDto.Image))
					{
						var fileName = FileHelper.GenerateFileName(teacherDto.Image.FileName);
						var path = Path.Combine(webHostEnvironment.WebRootPath, "avatars", fileName);
						using (var fileStream = new FileStream(path, FileMode.Create))
						{
							teacherDto.Image.CopyTo(fileStream);
						}
						teacher.Image = fileName;
					}
					else
					{
						return new { status = false, message = "File Invalid" };
					}
				
			}
			if (await dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(x => x.Email == teacher.Email) != null)
			{
				return new { status = false, message = "Email Already" };
			}
			var hashPassword = BCrypt.Net.BCrypt.HashPassword(teacherDto.Password);
			if(await dbContext.Subjects.FindAsync(teacherDto.MajorSubjectId) == null)
			{
				return new { status = false, message = "Major Subject Does Not Exist" };
			}
			teacher.MajorSubject = await dbContext.Subjects.FindAsync(teacherDto.MajorSubjectId);
			if (await dbContext.Subjects.FindAsync(teacherDto.MinorSubjectId) == null)
			{
				return new { status = false, message = "Minor Subject Does Not Exist" };
			}
			teacher.MinorSubject = await dbContext.Subjects.FindAsync(teacherDto.MinorSubjectId);
			teacher.CreateBy = await dbContext.Users.
							FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString()));
			teacher.CreatedDate = DateTime.Now;
			teacher.UpdatedDate = DateTime.Now;
			teacher.Code = GenerateHelper.GenerateTeacherCode();
			dbContext.Teachers.Add(teacher);
			var mailHelper = new MailHelper(configuration);
			if (mailHelper.Send(configuration["Gmail:Username"], teacher.Email!, "Welcome " + teacher.FirstName+" "+teacher.LastName+ " courses at the Children's House", MailHelper.HtmlNewAccount(teacher.FirstName + " " + teacher.LastName, teacher.Email!, teacherDto.Password!)))
			{
				if (await dbContext.SaveChangesAsync() > 0)
				{
					user.Id = teacher.Id;
					user.FullName = teacher.FirstName + teacher.LastName;
					user.Email = teacher.Email;
					user.Password = hashPassword;
					user.Image = teacher.Image;
					user.Role = await dbContext.Roles.FindAsync(2);
					user.IsStatus = false;
					user.Wage = teacher.Wage;
					user.CreatedDate = DateTime.Now;
					user.UpdatedDate = DateTime.Now;
					dbContext.Users.Add(user);
					await dbContext.SaveChangesAsync();
					return new { status = true, message = "Ok" };
				}
				else
				{
					return new { status = false, message = "Failure" };
				}
			}

			return new { status = false, message = "Send Email Failure" };
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> UpdateAsync(Guid id, TeacherDto teacherDto)
	{
		var teacher = mapper.Map<Teacher>(teacherDto);
		try
		{
			var teacherModel = await dbContext.Teachers.FindAsync(id);
			if (teacherModel == null)
			{
				return new { status = false, message = "Teacher Does Not Exist" };
			}
			else
			{
				if (teacherDto.Image != null)
				{
					if (teacher.Image != teacherDto.Image.FileName)
					{
						if (FileHelper.IsImage(teacherDto.Image))
						{
							var pathDelete = Path.Combine(webHostEnvironment.WebRootPath, "avatars", teacher.Image);
							File.Delete(pathDelete);
							var fileName = FileHelper.GenerateFileName(teacherDto.Image.FileName);
							var path = Path.Combine(webHostEnvironment.WebRootPath, "avatars", fileName);
							using (var fileStream = new FileStream(path, FileMode.Create))
							{
								teacherDto.Image.CopyTo(fileStream);
							}
							teacher.Image = fileName;
						}
						else
						{
							return new { status = false, message = "File Invalid" };
						}
					}
				}

			}
			if (await dbContext.Subjects.FindAsync(teacherDto.MajorSubjectId) == null)
			{
				return new { status = false, message = "Major Subject Does Not Exist" };
			}
			teacher.MajorSubject = await dbContext.Subjects.FindAsync(teacherDto.MajorSubjectId);
			if (await dbContext.Subjects.FindAsync(teacherDto.MinorSubjectId) == null)
			{
				return new { status = false, message = "Minor Subject Does Not Exist" };
			}
			teacher.MinorSubject = await dbContext.Subjects.FindAsync(teacherDto.MinorSubjectId);
			if (await dbContext.Teachers.AsNoTracking().FirstOrDefaultAsync(x => x.Email == teacher.Email && x.Id != id) != null)
			{
				return new { status = false, message = "Email Already" };
			}
			teacherModel.FirstName = teacher.FirstName;
			teacherModel.LastName = teacher.LastName;
			teacherModel.PhoneNumber = teacher.PhoneNumber;
			teacherModel.Address = teacher.Address;
			teacherModel.Email = teacher.Email;
			teacherModel.Gender = teacher.Gender;
			teacherModel.DayOfBirth = teacher.DayOfBirth;
			teacherModel.TaxCode = teacher.TaxCode;
			teacherModel.MinorSubject = teacher.MinorSubject;
			teacherModel.MajorSubject = teacher.MajorSubject;
			teacherModel.Wage = teacher.Wage;
			dbContext.Entry(teacherModel).State = EntityState.Modified;
			if (await dbContext.SaveChangesAsync() > 0)
			{
				var user = await dbContext.Users.FindAsync(teacherModel.Id);
				user.Email = teacherModel.Email;
				user.FullName = teacherModel.FirstName + " " + teacherModel.LastName;
				user.Image = teacherModel.Image;
				user.Wage = teacherModel.Wage;
				dbContext.Entry(user).State = EntityState.Modified;
				await dbContext.SaveChangesAsync();
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
