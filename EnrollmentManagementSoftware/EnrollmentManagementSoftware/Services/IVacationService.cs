using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IVacationService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, VacationDto vacationDto);
	Task<dynamic> InsertAsync(VacationDto vacationDto);
	Task<dynamic> DeleteAsync(int id);
}
