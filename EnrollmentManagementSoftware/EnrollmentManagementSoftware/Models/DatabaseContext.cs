using Microsoft.EntityFrameworkCore;

namespace EnrollmentManagementSoftware.Models;

public class DatabaseContext : DbContext
{
	public DatabaseContext(DbContextOptions options) : base(options)
	{

	}

	public DbSet<AcademicYear> AcademicYears { set; get; }	
	public DbSet<Classroom> Classrooms { set; get; }
	public DbSet<Course> Courses { set; get; }
	public DbSet<Grade> Grades { set; get; }
	public DbSet<GradeType> GradeTypes { set; get; }
	public DbSet<GradingMethod> GradingMethods { set; get; }
	public DbSet<Permission> Permissions { set; get; }
	public DbSet<Role> Roles { set; get; }
	public DbSet<Room> Rooms { set; get; }
	public DbSet<Salary> Salaries { set; get; }
	public DbSet<Schedule> Schedules { set; get; }
	public DbSet<Student> Students { set; get; }
	public DbSet<Subject> Subjects { set; get; }
	public DbSet<SubjectGroup> SubjectGroups { set; get; }
	public DbSet<Teacher> Teachers { set; get; }
	public DbSet<TuitionPayment> TuitionPayment { set; get;}
	public DbSet<TuititionType> TuititionTypes { set; get; }
	public DbSet<User> Users { set; get; }
	public DbSet<Vacation> Vacations { set; get; }
}
