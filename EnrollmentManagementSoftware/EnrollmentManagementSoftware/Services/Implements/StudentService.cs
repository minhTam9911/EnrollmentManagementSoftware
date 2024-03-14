using AutoMapper;
using Castle.Core.Internal;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Helpers;
using EnrollmentManagementSoftware.Helplers;
using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace EnrollmentManagementSoftware.Services.Implements;

public class StudentService : IStudentService
{

	private DatabaseContext dbContext;
	private IMapper mapper;
	private IConfiguration configuration;
	private IWebHostEnvironment webHostEnvironment;
	private IHttpContextAccessor httpContextAccessor;
	public StudentService(DatabaseContext dbContext,IWebHostEnvironment webHostEnvironment, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
			if (await dbContext.Students.FindAsync(id) == null) {
				return new { status = false, message = "Id does not exist" };
			}
			else
			{
				dbContext.Students.Remove(await dbContext.Students!.FindAsync(id));
				if (await dbContext.SaveChangesAsync() > 0) {
					return new { status = true, message = "Ok" };
				}
				else
				{
					return new { status = false, message = "Failure" };
				}
			}
		}catch(Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetListAsync(int page, string? keyword)
	{
		try
		{
			if(await dbContext.Students.AnyAsync() == false) {
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				_ = keyword.IsNullOrEmpty() ? keyword = null : keyword!.ToLower();
				var pageResults = 10f;
				var pageCount = Math.Ceiling(await dbContext.Students.CountAsync() / pageResults);
				var students = await dbContext.Students
					.Where(x =>
							x.Code!.ToLower().Contains(keyword) ||
							x.LastName!.ToLower().Contains(keyword) ||
							x.Email!.ToLower().Contains(keyword) ||
							x.PhoneNumber!.Contains(keyword)
							)
					.Skip((page-1) * (int)pageResults)
					.Take((int)pageResults)
					.Select(x => new {
							id = x.Id,
							code = x.Code,
							firstName = x.FirstName,
							lastName = x.LastName,
							address = x.Address,
							phoneNumber = x.PhoneNumber,
							email = x.Email,
							parentName = x.ParentName,
							gender= x.Gender,
							image = x.Image,
							dateOfBirth = DateOnly.FromDateTime((DateTime)x.DayOfBirth!)
					})
					.ToListAsync();
				return new {

					pages = (int)pageCount,
					currentPage = page,
					status = true,
					message = "Ok",
					data = students
				};
			}
		}
		catch(Exception  ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetNewStudentsAsync(int page, string? keyword)
	{
		try
		{
			if (await dbContext.Students.AnyAsync() == false)
			{
				return new { status = false, message = "Data null" };
			}
			else
			{
				var pageResults = 10f;
				var pageCount = Math.Ceiling(await dbContext.Students.CountAsync() / pageResults);
				var students = await dbContext.Students
					.Where(x=>x.CreatedDate > DateTime.Now.AddDays(-30))
					.Skip((page - 1) * (int)pageResults)
					.Take((int)pageResults)
					.Select(x => new {
						id = x.Id,
						code = x.Code,
						firstName = x.FirstName,
						lastName = x.LastName,
						address = x.Address,
						phoneNumber = x.PhoneNumber,
						email = x.Email,
						parentName = x.ParentName,
						image = x.Image,
						dateOfBirth = DateOnly.FromDateTime((DateTime)x.DayOfBirth)
					})
					.ToListAsync();
				return new
				{
					status = true,
					message = "Ok",
					pages = (int)pageCount,
					currentPage = page,
					data = students
				};
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetStudentClassesAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task<dynamic> GetStudentDetailsAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public Task<dynamic> GetStudentScheduleAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public async Task<dynamic> InsertAsync(StudentDto studentDto)
	{
		var student = mapper.Map<Student>(studentDto);
		var user = new User();
		try
		{
			if(studentDto.Image == null)
			{
				student.Image = "avatar-default-icon.png";
			}
			else
			{
				
				if(FileHelper.IsImage(studentDto.Image))
				{
					var fileName = FileHelper.GenerateFileName(studentDto.Image.FileName);
					var path = Path.Combine(webHostEnvironment.WebRootPath, "avatars", fileName);
					using (var fileStream = new FileStream(path, FileMode.Create))
					{
						studentDto.Image.CopyTo(fileStream);
					}
					student.Image = fileName;
				}
				else
				{
					return new { status = false, message = "File Invalid" };
				}
			}
			var hashPassword = BCrypt.Net.BCrypt.HashPassword(studentDto.Password);

			if(await dbContext.Classrooms.FindAsync(studentDto.ClassroomId) == null)
			{
				return new { status = false, message = "Classroom Does Not Exist" };
			}
			if (await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Email == student.Email) != null)
			{
				return new { status = false, message = "Email Already" };
			}
			student.Classroom = await dbContext.Classrooms.FindAsync(studentDto.ClassroomId);
			//student.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			student.CreatedDate = DateTime.Now;
			student.UpdatedDate = DateTime.Now;
			dbContext.Students.Add(student);
			var mailHelper = new MailHelper(configuration);
			if (mailHelper.Send(configuration["Gmail:Username"], student.Email!, "Welcome "+student.FirstName+" "+student.LastName+ " courses at the Children's House", MailHelper.HtmlNewAccount(student.FirstName + " " + student.LastName, student.Email!, studentDto.Password!))){
				if(await dbContext.SaveChangesAsync() > 0)
				{
					user.Id = student.Id;
					user.FullName = student.FirstName + student.LastName;
					user.Email = student.Email;
					user.Password = hashPassword;
					user.Image = student.Image;
					user.Role = await dbContext.Roles.FindAsync(3);
					user.IsStatus = false;
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
		catch(Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public Task<dynamic> ProcessNewStudentTuitionAsync(Guid id)
	{
		throw new NotImplementedException();
	}

	public async Task<dynamic> UpdateAsync(Guid id, StudentDto studentDto)
	{
		var student = mapper.Map<Student>(studentDto);
		try
		{
			var studentModel = await dbContext.Students.FindAsync(id);


			if (studentModel == null)
			{
				return new { status = false, message = "Student Does Not Exist" };
			}
			else
			{

				if(studentDto.Image != null)
				{
					if (studentModel.Image != studentDto.Image.FileName)
					{
						if (FileHelper.IsImage(studentDto.Image))
						{
							var pathDelete = Path.Combine(webHostEnvironment.WebRootPath, "avatars", studentModel.Image);
							File.Delete(pathDelete);
							var fileName = FileHelper.GenerateFileName(studentDto.Image.FileName);
							var path = Path.Combine(webHostEnvironment.WebRootPath, "avatars", fileName);
							using (var fileStream = new FileStream(path, FileMode.Create))
							{
								studentDto.Image.CopyTo(fileStream);
							}
							student.Image = fileName;
						}
						else
						{
							return new { status = false, message = "File Invalid" };
						}
					}
				}
				
			}
			if(await dbContext.Classrooms.FindAsync(studentDto.ClassroomId) == null)
			{
				return new { status = false, message = "Classroom Does Not Exist" };

			}
			if (await dbContext.Students.AsNoTracking().FirstOrDefaultAsync(x => x.Email == student.Email && x.Id!=id) != null)
			{
				return new { status = false, message = "Email Already" };
			}
			student.Classroom = await dbContext.Classrooms.FindAsync(studentDto.ClassroomId);
			//student.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			student.UpdatedDate = DateTime.Now;
			studentModel.FirstName = student.FirstName;
			studentModel.LastName = student.LastName;
			studentModel.PhoneNumber = student.PhoneNumber;
			studentModel.Address = student.Address;
			studentModel.Email = student.Email;
			studentModel.Gender = student.Gender;
			studentModel.DayOfBirth = student.DayOfBirth;
			studentModel.ParentName = student.ParentName;
			studentModel.Image = student.Image;
			studentModel.Classroom = student.Classroom;
			dbContext.Entry(studentModel).State = EntityState.Modified;
				if (await dbContext.SaveChangesAsync() > 0)
				{
				var user = await dbContext.Users.FindAsync(studentModel.Id);
				user.Email = studentModel.Email;
				user.FullName = studentModel.FirstName + " " + studentModel.LastName;
				user.Image = studentModel.Image;
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
