﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnrollmentManagementSoftware.Models;

public partial class Student
{
	[Key]
	public Guid Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Address { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Email { get; set; }
	public bool? Gender { get; set; }
	public string? Password { get; set; }
	public DateTime? DayOfBirth { get; set; }
	public string? Avatar { get; set; }
	public string? ParentName { get; set; }
	public virtual Classroom? Classroom { get;set;}
	public DateTime? CreatedDate { get; set; }
	public DateTime? UpdatedDate { get; set; }
}
