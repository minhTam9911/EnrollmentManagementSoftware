using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IGradeService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, GradeDto gradeDto);
	Task<dynamic> InsertAsync(GradeDto gradeDto);
	Task<dynamic> DeleteAsync(int id);
}
