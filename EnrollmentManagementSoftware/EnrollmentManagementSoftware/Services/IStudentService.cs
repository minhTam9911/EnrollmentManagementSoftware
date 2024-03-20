using EnrollmentManagementSoftware.DTOs;

namespace EnrollmentManagementSoftware.Services;

public interface IStudentService
{
	Task<dynamic> GetListAsync(int page, string? keyword);
	Task<dynamic> GetNewStudentsAsync(int page, string? keyword);
	Task<dynamic> InsertAsync(StudentDto studentDto);
	Task<dynamic> GetStudentClassesAsync(Guid id);
	Task<dynamic> ProcessNewStudentTuitionAsync(Guid id);
	Task<dynamic> GetStudentScheduleAsync(Guid id);
	Task<dynamic> GetStudentDetailsAsync(Guid id);
	Task<dynamic> UpdateAsync(Guid id, StudentDto studentDto);
	Task<dynamic> DeleteAsync(Guid id);
	Task<dynamic> RegisterClassroomAsync(Guid idStudent, int idClassroom);

}
