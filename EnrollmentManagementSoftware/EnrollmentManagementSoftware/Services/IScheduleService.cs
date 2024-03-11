using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IScheduleService
{
	Task<dynamic> GetListAsync();
	Task<dynamic> GetAsync(int id);
	Task<dynamic> GetByTeacherAsync(Guid idTeacher);
	Task<dynamic> GetByStudentAsync(Guid idStudent);
	Task<dynamic> UpdateAsync(int id, ScheduleDto scheduleDto);
	Task<dynamic> InsertAsync(ScheduleDto scheduleDto);
	Task<dynamic> DeleteAsync(int id);
}
