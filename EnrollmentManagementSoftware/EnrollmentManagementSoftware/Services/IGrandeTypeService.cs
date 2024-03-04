using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IGrandeTypeService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, GradeTypeDto gradeTypeDto);
	Task<dynamic> InsertAsync(GradeTypeDto gradeTypeDto);
	Task<dynamic> DeleteAsync(int id);
}
