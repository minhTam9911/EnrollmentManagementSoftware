using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnrollmentManagementSoftware.Services.Implements;

public class RoomService : IRoomService
{
	private DatabaseContext dbContext;
	private IMapper mapper;
	private IHttpContextAccessor httpContextAccessor;
	public RoomService(DatabaseContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
	{
		this.dbContext = dbContext;
		this.mapper = mapper;
		this.httpContextAccessor = httpContextAccessor;
	}

	public async Task<dynamic> DeleteAsync(int id)
	{
		try
		{
			if (await dbContext.Rooms.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			dbContext.Rooms.Remove(await dbContext.Rooms.FindAsync(id));
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
			if (await dbContext.Rooms.FindAsync(id) == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}

			var room = await dbContext.Rooms.Where(x => x.Id == id).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				facilities = x.Facilities,
				capacity = x.Capacity,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).FirstOrDefaultAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = room
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
			if (await dbContext.Rooms.Where(x => x.Name!.ToLower().Contains(name.ToLower())).AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var rooms = await dbContext.Rooms.Where(x => x.Name!.ToLower().Contains(name.ToLower())).Select(x => new
			{
				id = x.Id,
				name = x.Name,
				facilities = x.Facilities,
				capacity = x.Capacity,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = rooms
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
			if (await dbContext.Rooms.AnyAsync() == false)
			{
				return new { status = false, message = "Data Is Null" };
			}

			var rooms = await dbContext.Rooms.Select(x => new
			{
				id = x.Id,
				name = x.Name,
				facilities = x.Facilities,
				capacity = x.Capacity,
				createdDate = x.CreatedDate,
				updatedDate = x.UpdatedDate,
				createBy = x.CreateBy.FullName,
			}).ToListAsync();
			return new
			{
				status = true,
				message = "Ok",
				data = rooms
			};
		}
		catch (Exception ex)
		{
			return new { status = false, message = ex.Message };
		}
	}

	public async Task<dynamic> InsertAsync(RoomDto roomDto)
	{
		var room = mapper.Map<Room>(roomDto);
		try
		{
			room.CreatedDate = DateTime.Now;
			room.UpdatedDate = DateTime.Now;
			room.CreateBy = await dbContext.Users.
							FindAsync(Guid.Parse(httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name).ToString()));
			await dbContext.Rooms.AddAsync(room);
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

	public async Task<dynamic> UpdateAsync(int id, RoomDto roomDto)
	{
		var room = mapper.Map<Room>(roomDto);
		try
		{
			var roomModel = await dbContext.Rooms.FindAsync(id);
			if (roomModel == null)
			{
				return new { status = false, message = "Id Does Not Exist" };
			}
			room.UpdatedDate = DateTime.Now;
			roomModel.Name = room.Name;
			roomModel.Capacity = room.Capacity;
			roomModel.Facilities = room.Facilities;
			roomModel.UpdatedDate = room.UpdatedDate;
			dbContext.Entry(roomModel).State = EntityState.Modified;
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
