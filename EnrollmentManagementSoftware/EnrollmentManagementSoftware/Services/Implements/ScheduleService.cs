using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace EnrollmentManagementSoftware.Services.Implements;

public class ScheduleService : IScheduleService
{

	private DatabaseContext dbContext;
	private IMapper mapper;
	private IConfiguration configuration;
	private IWebHostEnvironment webHostEnvironment;
	private IHttpContextAccessor httpContextAccessor;
	public ScheduleService(DatabaseContext dbContext, IWebHostEnvironment webHostEnvironment, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.webHostEnvironment = webHostEnvironment;
		this.mapper = mapper;
		this.configuration = configuration;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.Schedules.FindAsync(id) == null)
			{
				return new { status = false, message = "Id does not exist" };
			}
			else
			{
				dbContext.Schedules.Remove(await dbContext.Schedules!.FindAsync(id));
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

	public async Task<dynamic> GetAsync(int id)
	{
		try
		{
			if (await dbContext.Schedules.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				/*_ = keyword.IsNullOrEmpty() ? keyword = null : keyword!.ToLower();
				var pageResults = 10f;
				var pageCount = Math.Ceiling(await dbContext.Students.CountAsync() / pageResults);*/
				var schedule = await dbContext.Schedules
					.Where(x => x.Id == id)
					.Select(x => new
					{
						id = x.Id,
						classroom = new
						{
							id = x.Classroom.Id,
							code = x.Classroom.Code,
							name = x.Classroom.Name
						},
						subject = new
						{
							id = x.Subject.Id,
							name = x.Subject.Name
						},
						startTime = x.StartTime,
						endTime = x.EndTime,
						startDate = x.StartDate,
						endDate = x.EndDate,
						days = x.Days.Select(i => new
						{
							name = i.Name
						}),
						room = new
						{
							id = x.Room.Id,
							name = x.Room.Name
						},
						teacher = new
						{
							id = x.Teacher.Id,
							firstName = x.Teacher.FirstName,
							lastName = x.Teacher.LastName
						},
						//createBy = x.CreateBy.FullName

					})
					.FirstOrDefaultAsync();
				return new
				{

					status = true,
					message = "Ok",
					data = schedule
				};
			}
		} 
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetByStudentAsync(Guid idStudent)
	{
		try
		{
			if (await dbContext.Schedules.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				List<Schedule> schedules = new List<Schedule>();

				var student = await dbContext.Students.FindAsync(idStudent);

				ICollection<Schedule>? schedulesClassroom = student!.Classroom!.Schedules;

				for(int i = 0; i < schedulesClassroom!.Count; i++)
				{
					for(int j = i+1; j < schedulesClassroom.Count; j++)
					{
						if(schedulesClassroom.ElementAt(i).Subject.Name == schedulesClassroom.ElementAt(j).Subject.Name)
						{
							if (schedulesClassroom.ElementAt(i).StartTime != schedulesClassroom.ElementAt(j).StartTime)
							{
								if (schedulesClassroom.ElementAt(i).StartDate != schedulesClassroom.ElementAt(j).StartDate)
								{
									schedules.Add(schedulesClassroom.ElementAt(i));
								}
							}
						}
						else
						{
							schedules.Add(schedulesClassroom.ElementAt(i));
						}
					}
				}

				var scheduleByStudent = schedules
					.Select(x => new
					{
						id = x.Id,
						classroom = new
						{
							id = x.Classroom.Id,
							code = x.Classroom.Code,
							name = x.Classroom.Name
						},
						subject = new
						{
							id = x.Subject.Id,
							name = x.Subject.Name
						},
						startTime = x.StartTime,
						endTime = x.EndTime,
						startDate = x.StartDate,
						endDate = x.EndDate,
						days = x.Days.Select(i => new
						{
							name = i.Name
						}),
						room = new
						{
							id = x.Room.Id,
							name = x.Room.Name
						},
						teacher = new
						{
							id = x.Teacher.Id,
							firstName = x.Teacher.FirstName,
							lastName = x.Teacher.LastName
						},
						//createBy = x.CreateBy.FullName

					}).ToList();
				return new
				{

					status = true,
					message = "Ok",
					data = scheduleByStudent
				};
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> GetByTeacherAsync(Guid idTeacher)
	{
		try
		{
			if (await dbContext.Schedules.Where(x=>x.Teacher.Id == idTeacher).AnyAsync()== false)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{
				
				var scheduleByTeacher = await dbContext.Schedules.Where(x => x.Teacher.Id == idTeacher)
					.Select(x => new
					{
						id = x.Id,
						classroom = new
						{
							id = x.Classroom.Id,
							code = x.Classroom.Code,
							name = x.Classroom.Name
						},
						subject = new
						{
							id = x.Subject.Id,
							name = x.Subject.Name
						},
						startTime = x.StartTime,
						endTime = x.EndTime,
						startDate = x.StartDate,
						endDate = x.EndDate,
						days = x.Days.Select(i => new
						{
							name = i.Name
						}),
						room = new
						{
							id = x.Room.Id,
							name = x.Room.Name
						},
						teacher = new
						{
							id = x.Teacher.Id,
							firstName = x.Teacher.FirstName,
							lastName = x.Teacher.LastName
						},
						//createBy = x.CreateBy.FullName

					}).ToListAsync();
				return new
				{

					status = true,
					message = "Ok",
					data = scheduleByTeacher
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
			if (await dbContext.Schedules.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}
			else
			{

				var schedules = await dbContext.Schedules
					.Select(x => new
					{
						id = x.Id,
						classroom = new
						{
							id = x.Classroom.Id,
							code = x.Classroom.Code,
							name = x.Classroom.Name
						},
						subject = new
						{
							id = x.Subject.Id,
							name = x.Subject.Name
						},
						startTime = x.StartTime,
						endTime = x.EndTime,
						startDate = x.StartDate,
						endDate = x.EndDate,
						days = x.Days.Select(i => new
						{
							name = i.Name
						}),
						room = new
						{
							id = x.Room.Id,
							name = x.Room.Name
						},
						teacher = new
						{
							id = x.Teacher.Id,
							firstName = x.Teacher.FirstName,
							lastName = x.Teacher.LastName
						},
						//createBy = x.CreateBy.FullName

					}).ToListAsync();
				return new
				{

					status = true,
					message = "Ok",
					data = schedules
				};
			}
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(ScheduleDto scheduleDto)
	{
		var schedule = mapper.Map<Schedule>(scheduleDto);
		try
		{
			

			if(await dbContext.Classrooms.FindAsync(scheduleDto.ClassroomId) == null)
			{
				return new { status = false, message = "Classroom does not exist" };
			}
			schedule.Classroom = await dbContext.Classrooms.FindAsync(scheduleDto.ClassroomId);
			if (await dbContext.Subjects.FindAsync(scheduleDto.SubjectId) == null)
			{
				return new { status = false, message = "Subject does not exist" };
			}
			schedule.Subject = await dbContext.Subjects.FindAsync(scheduleDto.SubjectId);
			if (await dbContext.Rooms.FindAsync(scheduleDto.RoomId) == null)
			{
				return new { status = false, message = "Room does not exist" };
			}
			schedule.Room = await dbContext.Rooms.FindAsync(scheduleDto.RoomId);
			if (await dbContext.Teachers.FindAsync(scheduleDto.TeacherId) == null)
			{
				return new { status = false, message = "Teacher does not exist" };
			}
			schedule.Teacher = await dbContext.Teachers.FindAsync(scheduleDto.TeacherId);
			var checkSchedule = await dbContext.Schedules.Where(x => x.Teacher.Id == scheduleDto.TeacherId).ToListAsync();
			foreach (var i in checkSchedule)
			{
				if (i.StartDate <= schedule.StartDate && i.EndDate >= schedule.EndDate)
				{
					if (i.StartTime <= schedule.StartTime && i.EndTime >= schedule.EndTime)
					{
						foreach (var j in i.Days)
						{
							foreach (var k in schedule.Days)
							{
								if (k.Name == j.Name)
								{
									return new
									{
										status = false,
										message = "Teach exist schedule at " + k.Name + ", between time " + i.StartTime + "-" + i.EndDate + ", in " + schedule.StartDate + " to " + schedule.EndDate,
									};
								}
							}
						}
					}
				}
			}
			schedule.CreatedDate = DateTime.Now;
			schedule.UpdatedDate = DateTime.Now;
			//schedule.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			await dbContext.Schedules.AddAsync(schedule);
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

	public async Task<dynamic> UpdateAsync(int id, ScheduleDto scheduleDto)
	{
		var schedule = mapper.Map<Schedule>(scheduleDto);
		try
		{
			var scheduleModel = await dbContext.Schedules.FindAsync(id);
			if (scheduleModel == null)
			{
				return new { status = false, message = "Schedule does not exist" };
			}
			if (await dbContext.Classrooms.FindAsync(scheduleDto.ClassroomId) == null)
			{
				return new { status = false, message = "Classroom does not exist" };
			}
			schedule.Classroom = await dbContext.Classrooms.FindAsync(scheduleDto.ClassroomId);
			if (await dbContext.Subjects.FindAsync(scheduleDto.SubjectId) == null)
			{
				return new { status = false, message = "Subject does not exist" };
			}
			schedule.Subject = await dbContext.Subjects.FindAsync(scheduleDto.SubjectId);
			if (await dbContext.Rooms.FindAsync(scheduleDto.RoomId) == null)
			{
				return new { status = false, message = "Room does not exist" };
			}
			schedule.Room = await dbContext.Rooms.FindAsync(scheduleDto.RoomId);
			if (await dbContext.Teachers.FindAsync(scheduleDto.TeacherId) == null)
			{
				return new { status = false, message = "Teacher does not exist" };
			}
			schedule.Teacher = await dbContext.Teachers.FindAsync(scheduleDto.TeacherId);
			var checkSchedule = await dbContext.Schedules.Where(x => x.Teacher.Id == scheduleDto.TeacherId && x.Id !=id).ToListAsync();
			foreach (var i in checkSchedule)
			{
				if (i.StartDate <= schedule.StartDate && i.EndDate >= schedule.EndDate)
				{
					if (i.StartTime <= schedule.StartTime && i.EndTime >= schedule.EndTime)
					{
						foreach (var j in i.Days)
						{
							foreach (var k in schedule.Days)
							{
								if (k.Name == j.Name)
								{
									return new
									{
										status = false,
										message = "Teach exist schedule at " + k.Name + ", between time " + i.StartTime + "-" + i.EndDate + ", in " + schedule.StartDate + " to " + schedule.EndDate,
									};
								}
							}
						}
					}
				}
			}
			scheduleModel.StartDate= schedule.StartDate;
			scheduleModel.EndDate = schedule.EndDate;
			scheduleModel.EndTime = schedule.EndTime;
			scheduleModel.StartTime = schedule.StartTime;
			scheduleModel.Classroom = schedule.Classroom;
			scheduleModel.Room = schedule.Room;
			scheduleModel.Subject = schedule.Subject;
			scheduleModel.Teacher = schedule.Teacher;
			schedule.UpdatedDate = DateTime.Now;
			//schedule.CreateBy = await dbContext.Users.
			//					FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name).ToString()));
			dbContext.Entry(scheduleModel).State = EntityState.Modified;
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
