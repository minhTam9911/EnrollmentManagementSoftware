using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IGrandingMethodService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, GradingMethodDto gradingMethodDto);
	Task<dynamic> InsertAsync(GradingMethodDto gradingMethodDto);
	Task<dynamic> DeleteAsync(int id);
}
