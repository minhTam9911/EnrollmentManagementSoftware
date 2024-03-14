using AutoMapper;
using EnrollmentManagementSoftware.DTOs;
using EnrollmentManagementSoftware.Models;
using System.Globalization;

namespace EnrollmentManagementSoftware.Configurations;

public class AutoMapperProfile : Profile
{
	public AutoMapperProfile()
	{
		CreateMap<AcademicYearDto, AcademicYear>()
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue)));
		CreateMap<AcademicYear, AcademicYearDto>()
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.StartDate.Value)))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.EndDate.Value)));
		CreateMap<ClassroomDto, Classroom>();
		CreateMap<Classroom, ClassroomDto>();
		CreateMap<CourseDto, Course>();
		CreateMap<Course, CourseDto>();
		CreateMap<Grade, GradeDto>();
		CreateMap<GradeTypeDto, GradeType>();
		CreateMap<GradingMethodDto, GradingMethod>();
		CreateMap<PermissionDto, Permission>();
		CreateMap<RoleDto, Role>();
		CreateMap<RoomDto, Room>();
		CreateMap<SalaryDto,Salary>();
		CreateMap<ScheduleDto, Schedule>()
			.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime.ToTimeSpan()))
			.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime.ToTimeSpan()))
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue)))
			.ForMember(dest => dest.Days, opt => opt.MapFrom(src => src.Days.Select(x => new Day {Name =  x.DayOfWeek.ToString() })));
		CreateMap<StudentDto, Student>()
			//.ForMember(dest => dest.DayOfBirth, opt => opt.MapFrom(src => src.DayOfBirth.ToDateTime(TimeOnly.MinValue)))
			.ForMember(dest => dest.Image, opt => opt.Ignore());
		CreateMap<SubjectDto, Subject>();
		
		CreateMap<SubjectGroupDto, SubjectGroup>();

		CreateMap<TeacherDto, Teacher>();
			//.ForMember(dest => dest.DayOfBirth, opt => opt.MapFrom(src => src.DayOfBirth.ToDateTime(TimeOnly.MinValue)));
		CreateMap<TuitionPaymentDto, TuitionPayment>();
		CreateMap<TuitionTypeDto, TuitionType>();
		CreateMap<UserDto, User>()
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()));
		CreateMap<VacationDto, Vacation>()
			.ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToDateTime(TimeOnly.MinValue)))
			.ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToDateTime(TimeOnly.MinValue)));
	}
}
