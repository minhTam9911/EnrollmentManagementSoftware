using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IAcademicYearService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, AcademicYearDto academicYearDto);
	Task<dynamic> InsertAsync(AcademicYearDto academicYearDto);
	Task<dynamic> DeleteAsync(int id);
}
