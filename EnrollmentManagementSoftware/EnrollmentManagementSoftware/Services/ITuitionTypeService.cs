using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface ITuitionTypeService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, TuitionTypeDto tuitionTypeDto);
	Task<dynamic> InsertAsync(TuitionTypeDto tuitionTypeDto);
	Task<dynamic> DeleteAsync(int id);
}
