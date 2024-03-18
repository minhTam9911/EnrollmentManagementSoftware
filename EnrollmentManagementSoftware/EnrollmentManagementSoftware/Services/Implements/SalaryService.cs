using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnrollmentManagementSoftware.Services.Implements;

public class SalaryService : ISalaryService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public SalaryService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}
	public async Task<dynamic> DeleteAsync(Guid id)
	{
		try
		{
			if (await dbContext.Salaries.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.Salaries.Remove(await dbContext.Salaries.FindAsync(id));
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
			if (await dbContext.Salaries.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var salary = await dbContext.Salaries.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				wagePercent = x.WagePercent,
				subsidizeName = x.SubsidizeName,
				subsidize = x.Subsidize,
				description = x.Description,
				totalAmount = x.TotalAmount,
				employee = new
				{
					fullName = x.Employee.FullName

				},
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = salary
			};
		}
		catch (Exception ex)
		{
			return new
			{
				status = false,
				message = ex.Message
			};
		}
	}

	public async Task<dynamic> GetListAsync()
	{
		try
		{
			if (await dbContext.Salaries.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var salaries = await dbContext.Salaries.Select(x => new
			{
				id = x.Id,
				wagePercent = x.WagePercent,
				subsidizeName = x.SubsidizeName,
				subsidize = x.Subsidize,
				description = x.Description,
				totalAmount = x.TotalAmount,
				employee = new
				{
					fullName= x.Employee.FullName

				},
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = salaries
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(SalaryDto salaryDto)
	{
		var salary = mapper.Map<Salary>(salaryDto);
		try
		{
			salary.CreatedDate = DateTime.Now;
			salary.UpdatedDate = DateTime.Now;
			salary.IsStatus = false;
			salary.CreateBy = await dbContext.Users.
							FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString()));

			if (await dbContext.Users.FindAsync(salaryDto.EmployeeId) == null)
			{
				return new { status = false, message = "Eployee Does Not Exist" };
			}
			salary.Employee = await dbContext.Users.FindAsync(salaryDto.EmployeeId);

			
			if (salary.Employee.Role.Name == "Teacher")
			{
				decimal statisticalCourse = 0;
				var tutitionPayment = await dbContext.TuitionPayments.Where(x => x.CreatedDate.Value.Day >= 1 && x.CreatedDate.Value.Month == (DateTime.Now.Month-1) && x.CreatedDate.Value.Year == DateTime.Now.Year && x.IsStatus == true).ToListAsync();
				if (await dbContext.Courses.FindAsync(salaryDto.CourseId) == null)
				{
					return new { status = false, message = "Course Does Not Exist" };
				}
				salary.Course = await dbContext.Courses.FindAsync(salaryDto.CourseId);
				foreach (var i in tutitionPayment)
				{
					if(i.Classroom.Course.Id == salary.Course.Id)
					{
						statisticalCourse +=i.Amount.GetValueOrDefault();
					}
				}
				decimal totalWage = (statisticalCourse * salary.WagePercent.GetValueOrDefault())/100 + salary.Subsidize.GetValueOrDefault();
				salary.TotalAmount = totalWage;
			}
			else
			{
				salary.TotalAmount = salary.Employee.Wage + salary.Subsidize;
			}

			await dbContext.Salaries.AddAsync(salary);
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

	public async Task<dynamic> UpdateAsync(int id, bool status)
	{
		var salary = await dbContext.Salaries.FindAsync(id);
		if (salary == null)
		{
			return new { status = false, message = "Salary Does Not Exist" };
		}
		salary.IsStatus = status;
		dbContext.Entry(salary).State = EntityState.Modified;
		await dbContext.SaveChangesAsync();
		return new { status = true, message = "Ok" };
	}

}
