namespace EnrollmentManagementSoftware.Helpers;

public class FileHelper
{
	public static string GenerateFileName(string fileName)
	{
		var name = Guid.NewGuid().ToString().Replace("-", "");
		var lastIndex = fileName.LastIndexOf('.');
		var extend = fileName.Substring(lastIndex);
		return name + extend;
		//return name+ "." + extend;
	}
	public static bool IsImage(IFormFile file)
	{
		string[] fileExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
		var check = fileExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant());
		var checkSize = file.Length;
		if (check && checkSize <= 50 * 1024 * 1024)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
