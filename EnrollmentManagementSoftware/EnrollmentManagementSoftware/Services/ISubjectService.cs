using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface ISubjectService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, SubjectDto subjectDto);
	Task<dynamic> InsertAsync(SubjectDto subjectDto);
	Task<dynamic> DeleteAsync(int id);

}
