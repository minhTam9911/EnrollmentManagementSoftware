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
	public DbSet<TuitionPayment> TuitionPayments { set; get;}
	public DbSet<TuitionType> TuitionTypes { set; get; }
	public DbSet<User> Users { set; get; }
	public DbSet<Vacation> Vacations { set; get; }
	

	/*protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Permission>().HasData(
			new Permission { Id = 1, Name = "System", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
		); // Seed Permission first

		modelBuilder.Entity<Role>().HasData(
			new Role
			{
				Id = 1,
				Name = "SuperAdmin",
				Permissions = new Permission[] {
            // Refrerence existing Permission with RoleId
            new Permission { Id = 1, Name = "System", RoleId = 1 }
			}
			});

		modelBuilder.Entity<User>().HasData(
			new User
			{
				Id = Guid.NewGuid(),
				FullName = "SuperAdmin",
				Email = "Minhtamceo1@gmail.com",
				Password = BCrypt.Net.BCrypt.HashPassword("abc123"),
				Image = "default-avatar.jpg",
				IsStatus = true,
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				Role = new Role { Id = 1, Name = "SuperAdmin" } // No need for Permissions here
			}
		);

		base.OnModelCreating(modelBuilder);
	}*/

}
