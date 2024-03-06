namespace EnrollmentManagementSoftware.DTOs;

public class PageDto<T>
{
	public IReadOnlyList<T>? Pages { get;}
	public int? TotalRecords { get; set; }
	public PageDto(IReadOnlyList<T>? pages, int? totalRecords)
	{
		Pages = pages;
		TotalRecords = totalRecords;
	}
}
