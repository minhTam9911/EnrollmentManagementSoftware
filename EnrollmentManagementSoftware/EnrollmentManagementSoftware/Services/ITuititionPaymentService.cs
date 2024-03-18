using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface ITuititionPaymentService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(Guid id);
	Task<dynamic> InsertAsync(TuitionPaymentDto tuitionPaymentDto);
	Task<dynamic> DeleteAsync(Guid id);
	Task<dynamic> UpdateAsync(Guid id, bool status);

}
