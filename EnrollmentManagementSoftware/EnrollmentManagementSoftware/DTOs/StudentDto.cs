﻿using AutoMapper.Configuration.Annotations;
using EnrollmentManagementSoftware.Models;
using System.ComponentModel.DataAnnotations;

namespace EnrollmentManagementSoftware.DTOs;

public class StudentDto
{
	[Required]
	public string? FirstName { get; set; }
	[Required]
	public string? LastName { get; set; }
	[Required]
	public string? Address { get; set; }
	[Required]
	[Phone]
	public string? PhoneNumber { get; set; }
	[Required]
	[EmailAddress]
	public string? Email { get; set; }
	[Required]
	public bool? Gender { get; set; }
	[Required]
	public string? Password { get; set; }
	[Required]
	public DateTime DayOfBirth { get; set; }
	[Required]
	public string? ParentName { get; set; }
	[Required]
	public int? ClassroomId { get; set; }
	[Ignore]
	public IFormFile? Image { get; set; }
}
