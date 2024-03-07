using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface ITeacherService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(Guid id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(Guid id, TeacherDto teacherDtoto);
	Task<dynamic> InsertAsync(TeacherDto teacherDto);
	Task<dynamic> DeleteAsync(Guid id);
}
