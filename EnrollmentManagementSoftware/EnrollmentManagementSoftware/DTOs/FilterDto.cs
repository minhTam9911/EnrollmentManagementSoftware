namespace EnrollmentManagementSoftware.DTOs;

public class FilterDto
{
	public string? Keyword { get; set; }
	public string? OrderBy { get; set; }
	public bool? Descending { get; set; }
	public int? PageIndex { get; set; }
	public int? PageSize { get; set; }
}
