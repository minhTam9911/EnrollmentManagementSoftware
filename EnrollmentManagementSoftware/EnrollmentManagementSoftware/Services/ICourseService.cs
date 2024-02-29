using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface ICourseService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetDetailAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, CourseDto courseDto);
	Task<dynamic> InsertAsync(CourseDto courseDto);
	Task<dynamic> DeleteAsync(int id);
}
