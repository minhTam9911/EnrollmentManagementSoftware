using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface ISubjectGroupService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetDetailAsync(int id);
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, SubjectGroupDto subjectGroupDto);
	Task<dynamic> InsertAsync(SubjectGroupDto subjectGroupDto);
	Task<dynamic> DeleteAsync(int id);
}
