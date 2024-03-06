using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IUserService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(Guid id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(Guid id, UserDto userDto);
	Task<dynamic> InsertAsync(UserDto userDto);
	Task<dynamic> DeleteAsync(Guid id);
	
}
