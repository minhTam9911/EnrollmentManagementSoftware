using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IRoomService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, RoomDto roomDto);
	Task<dynamic> InsertAsync(RoomDto roomDto);
	Task<dynamic> DeleteAsync(int id);
}
