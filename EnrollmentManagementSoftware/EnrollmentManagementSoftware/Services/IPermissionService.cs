using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IPermissionService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, PermissionDto permissionDto);
	Task<dynamic> InsertAsync(PermissionDto permissionDto);
	Task<dynamic> DeleteAsync(int id);
}
