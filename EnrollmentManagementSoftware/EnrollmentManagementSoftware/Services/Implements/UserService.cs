using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Helpers;
using EnrollmentManagementSoftware.Helplers;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace EnrollmentManagementSoftware.Services.Implements;

public class UserService : IUserService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IConfiguration configuration;
	private IWebHostEnvironment webHostEnvironment;
	private IHttpContextAccessor httpContextAccessor;
	public UserService(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
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
			if (await dbContext.Users.FindAsync(id) == null)
			{
				return new { status = false, message = "Id does not exist" };
			}
			else
			{
				dbContext.Users.Remove(await dbContext.Users!.FindAsync(id));
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
			var users = await dbContext.Users.Where(x => x.Role!.Name!.ToLower() != "student" && x.Role.Name.ToLower() != "teacher").ToListAsync();
			if (users == null)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				if (users.FirstOrDefault(x => x.Id == id) !=null)
				{
					var result = users.Where(x => x.Id == id).Select(x => new {
						id = x.Id,
						fullName = x.FullName,
						email = x.Email,
						role = new
						{
							id = x.Role.Id,
							name = x.Role.Name
						},
						image = x.Image,
						status = x.IsStatus,
						createdDate = x.CreatedDate,
						updatedDate = x.UpdatedDate
					}).FirstOrDefault();
					return new
					{

						status = true,
						message = "Ok",
						data = result
					};
				}
				else
				{
					return new { status = false, message = "Data Is Null" };
				}

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
			var users = await dbContext.Users.Where(x => x.Role!.Name!.ToLower() != "student" && x.Role.Name.ToLower() != "teacher").ToListAsync();
			if (users == null)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				if(users.Where(x=>x.FullName.ToLower().Contains(name.ToLower())).Any())
				{
					var results = users.Where(x => x.FullName.ToLower().Contains(name.ToLower())).Select(x => new {
						id = x.Id,
						fullName = x.FullName,
						email = x.Email,
						role = new
						{
							id = x.Role.Id,
							name = x.Role.Name
						},
						image = x.Image,
						status = x.IsStatus,
						createdDate = x.CreatedDate,
						updatedDate = x.UpdatedDate
					}).ToList();
					return new
					{

						status = true,
						message = "Ok",
						data = results
					};
				}
				else
				{
					return new { status = false, message = "Data Is Null" };
				}
				
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
			if (await dbContext.Users.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				var users = await dbContext.Users.Where(x=>x.Role!.Name!.ToLower() != "student" && x.Role.Name.ToLower() != "teacher")
					.Select(x => new {
						id = x.Id,
						fullName = x.FullName,
						email = x.Email,
						role = new
						{
							id = x.Role.Id,
							name = x.Role.Name
						},
						image = x.Image,
						status = x.IsStatus,
						createdDate = x.CreatedDate,
						updatedDate = x.UpdatedDate
					})
					.ToListAsync();
				return new
				{

					status = true,
					message = "Ok",
					data = users
				};
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(UserDto userDto)
	{
		var user = mapper.Map<User>(userDto);
		try
		{
			if (userDto.Image == null)
			{
				user.Image = "avatar-default-icon.png";
			}
			else
			{

				if (FileHelper.IsImage(userDto.Image))
				{
					var fileName = FileHelper.GenerateFileName(userDto.Image.FileName);
					var path = Path.Combine(webHostEnvironment.WebRootPath, "avatars", fileName);
					using (var fileStream = new FileStream(path, FileMode.Create))
					{
						userDto.Image.CopyTo(fileStream);
					}
					user.Image = fileName;
				}
				else
				{
					return new { status = false, message = "File Invalid" };
				}
			}
			var hashPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
			var roleHandle = await dbContext.Roles.FirstOrDefaultAsync(x=>x.Id == userDto.RoleId);
			if(roleHandle == null)
			{
				return new { status = false, message = "Role Does Not Exist" };
			}
			if(roleHandle.Name.ToLower().Equals("student") || roleHandle.Name.ToLower().Equals("teacher") || roleHandle.Name.ToLower().Equals("admin"))
			{
				return new { status = false, message = "Not Enough Authority" };
			}

			user.Role = roleHandle;
			user.CreatedDate = DateTime.Now;
			user.UpdatedDate = DateTime.Now;
			dbContext.Users.Add(user);
			var mailHelper = new MailHelper(configuration);
			if (mailHelper.Send(configuration["Gmail:Username"], user.Email!, "Welcome " +	user.FullName + " courses at the Children's House", MailHelper.HtmlNewAccount(user.FullName, user.Email!, userDto.Password!)))
			{
				if (await dbContext.SaveChangesAsync() > 0)
				{
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

	public async Task<dynamic> UpdateAsync(Guid id, UserDto userDto)
	{
		var user = mapper.Map<User>(userDto);
		try
		{
			var userModel = await dbContext.Users.FindAsync(id);
			if (userModel == null)
			{
				return new { status = false, message = "User Does Not Exist" };
			}
			else
			{
				if (userDto.Image != null)
				{
					if (userModel.Image != userDto.Image.FileName)
					{
						if (FileHelper.IsImage(userDto.Image))
						{
							var pathDelete = Path.Combine(webHostEnvironment.WebRootPath, "avatars", userDto.Image.FileName);
							File.Delete(pathDelete);
							var fileName = FileHelper.GenerateFileName(userDto.Image.FileName);
							var path = Path.Combine(webHostEnvironment.WebRootPath, "avatars", fileName);
							using (var fileStream = new FileStream(path, FileMode.Create))
							{
								userDto.Image.CopyTo(fileStream);
							}
							user.Image = fileName;
						}
						else
						{
							return new { status = false, message = "File Invalid" };
						}
					}
				}

			}
			var roleHandle = await dbContext.Roles.FirstOrDefaultAsync(x => x.Id == userDto.RoleId);
			if (roleHandle == null)
			{
				return new { status = false, message = "Role Does Not Exist" };
			}
			if (roleHandle.Name.ToLower().Equals("student") || roleHandle.Name.ToLower().Equals("teacher") || roleHandle.Name.ToLower().Equals("admin"))
			{
				return new { status = false, message = "Not Enough Authority" };
			}
			user.Role = roleHandle;
			userModel.UpdatedDate = DateTime.Now;
			userModel.Email = user.Email;
			userModel.FullName = user.FullName;
			userModel.Image = user.Image;
			userModel.Role = user.Role;
			userModel.IsStatus = user.IsStatus;
			dbContext.Entry(userModel).State = EntityState.Modified;
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
