using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnrollmentManagementSoftware.Services.Implements;

public class TuititionPaymentService : ITuititionPaymentService
{

	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public TuititionPaymentService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}
	public async Task<dynamic> DeleteAsync(Guid id)
	{
		try
		{
			if (await dbContext.TuitionPayments.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.TuitionPayments.Remove(await dbContext.TuitionPayments.FindAsync(id));
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

	public async Task<dynamic> GetAsync(Guid id)
	{
		try
		{
			if (await dbContext.TuitionPayments.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var tuitionPayment = await dbContext.TuitionPayments.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				discount = x.Discount,
				description = x.Description,
				paymentMethod = x.PaymentMethod,
				surcharge = x.Surcharge,
				amount = x.Amount,
				student = new
				{
					firstName = x.Student.FirstName,
					lastName = x.Student.LastName,

				},
				classroom = new
				{
					code = x.Classroom.Code,
					name = x.Classroom.Name
				},
				tuititionType = new
				{
					name = x.TuititionType.Name,
					percent = x.TuititionType.Percent
				},
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = tuitionPayment
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
			if (await dbContext.TuitionPayments.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var tuititionPayments = await dbContext.TuitionPayments.Select(x => new
			{
				id = x.Id,
				discount = x.Discount,
				description = x.Description,
				paymentMethod = x.PaymentMethod,
				surcharge = x.Surcharge,
				amount = x.Amount,
				student = new
				{
					firstName = x.Student.FirstName,
					lastName = x.Student.LastName,

				},
				classroom = new
				{
					code = x.Classroom.Code,
					name = x.Classroom.Name
				},
				tuititionType = new
				{
					name = x.TuititionType.Name,
					percent = x.TuititionType.Percent
				},
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = tuititionPayments
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(TuitionPaymentDto tuitionPaymentDto)
	{
		var tuitionPayment = mapper.Map<TuitionPayment>(tuitionPaymentDto);
		try
		{
			tuitionPayment.CreatedDate = DateTime.Now;
			tuitionPayment.UpdatedDate = DateTime.Now;
			tuitionPayment.IsStatus = false;
			tuitionPayment.CreateBy = await dbContext.Users.
							FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString()));

			if (await dbContext.TuitionTypes.FindAsync(tuitionPaymentDto.TuititionTypeId) == null)
			{
				return new { status = false, message = "Tuitition Type Does Not Exist" };
			}
			tuitionPayment.TuititionType = await dbContext.TuitionTypes.FindAsync(tuitionPaymentDto.TuititionTypeId);
			if (await dbContext.Classrooms.FindAsync(tuitionPaymentDto.ClassroomId) == null)
			{
				return new { status = false, message = "Classroom Does Not Exist" };
			}
			tuitionPayment.Classroom = await dbContext.Classrooms.FindAsync(tuitionPaymentDto.ClassroomId);
			if (await dbContext.Students.FindAsync(tuitionPaymentDto.StudentId) == null)
			{
				return new { status = false, message = "Student Does Not Exist" };
			}
			tuitionPayment.Student = await dbContext.Students.FindAsync(tuitionPaymentDto.StudentId);
			var amount = (tuitionPayment.Classroom.TuitionFee * tuitionPayment.TuititionType.Percent) / 100 
						- (tuitionPayment.Classroom.TuitionFee * tuitionPayment.Discount) / 100 
						+ tuitionPayment.Surcharge;
			tuitionPayment.Amount = amount;

			await dbContext.TuitionPayments.AddAsync(tuitionPayment);
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

	public async Task<dynamic> UpdateAsync(Guid id, bool status)
	{
		var tuititionPayment = await dbContext.TuitionPayments.FindAsync(id);
		if(tuititionPayment == null)
		{
			return new { status = false, message = "Payment Does Not Exist" };
		}tuititionPayment.IsStatus = status;
		tuititionPayment.UpdatedDate = DateTime.Now;
		dbContext.Entry(tuititionPayment).State = EntityState.Modified;
		await dbContext.SaveChangesAsync();
		return new { status = true, message = "Ok" };
	}
}
