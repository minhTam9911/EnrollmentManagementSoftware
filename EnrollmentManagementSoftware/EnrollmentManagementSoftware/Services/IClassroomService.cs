using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IClassroomService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, ClassroomDto classroomDto);
	Task<dynamic> InsertAsync(ClassroomDto classroomDto);
	Task<dynamic> DeleteAsync(int id);
}
