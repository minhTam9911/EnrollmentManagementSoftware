using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface ISalaryService
{

	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> InsertAsync(SalaryDto salaryDto);
	Task<dynamic> UpdateAsync(int id, bool status);
}
