namespace EnrollmentManagementSoftware.Helpers;

public class GenerateHelper
{

	public static string GenerateStudentCode()
	{
		string yearMonth = DateTime.Now.ToString("yyyyMM");
		string randomCode = Guid.NewGuid().ToString().Substring(0, 5);
		string studentCode = yearMonth + "-" + randomCode;
		return studentCode;
	}
	public static string GenerateTeacherCode()
	{
		string teacherCode = Guid.NewGuid().ToString().Substring(0, 6);
		return teacherCode;
	}

}
