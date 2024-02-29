using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IRoleService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByNameAsync(string name);
	Task<dynamic> UpdateAsync(int id, RoleDto roleDto);
	Task<dynamic> InsertAsync(RoleDto roleDto);
	Task<dynamic> DeleteAsync(int id);
	Task<dynamic> AddPermissionAsync(int roleId, List<int> permissionsId);
	Task<dynamic> DeletePermissionAsync(int roleId, List<int> permissionsId);
}
