	using EnrollmentManagementSoftware.Configurations;
using EnrollmentManagementSoftware.Converters;
using EnrollmentManagementSoftware.Models;
using EnrollmentManagementSoftware.Services;
using EnrollmentManagementSoftware.Services.Implements;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var connectString = builder.Configuration["Connection:DefaultString"];
builder.Services.AddDbContext<DatabaseContext>(option => option.UseLazyLoadingProxies().UseSqlServer(connectString), ServiceLifetime.Singleton);
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(options =>
{
	options.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
	options.JsonSerializerOptions.Converters.Add(new TimeOnlyConverter());

});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
	option.AddSecurityDefinition("oauth2", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
	{
		Description = "Standard Authorization Header Using The Bearer sheme(\"bearer {token}\")",
		In = Microsoft.OpenApi.Models.ParameterLocation.Header,
		Name = "Authorization",
		Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
	});
	option.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddCors(option =>
{
	option.AddPolicy("Open", policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IGrandingMethodService, GradingMethodService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<ISubjectGroupService, SubjectGroupService>();
builder.Services.AddScoped<ITuitionTypeService, TuitionTypeService>();
builder.Services.AddScoped<IVacationService, VacationService>();
builder.Services.AddScoped<IAcademicYearService, AcademicYearService>();
builder.Services.AddScoped<IClassroomService, ClassroomService>();
builder.Services.AddScoped<IGrandeTypeService, GradeTypeService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITuititionPaymentService, TuititionPaymentService>();
builder.Services.AddScoped<ISalaryService, SalaryService>();
builder.Services.AddSingleton<IAuthorizationHandler, IsPermissionWithRoleHandle>();
builder.Services.AddAuthorization(option =>
{
	option.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin","Manager"));
	option.AddPolicy("ReadClassPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Class.Read")));
	option.AddPolicy("ReadClassStudenPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Class.Student.Read")));
	option.AddPolicy("ReadClassSubjectPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Class.Subject.Read")));
	option.AddPolicy("CRUDClassPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Class.Create.Read.Update.Delete")));
	option.AddPolicy("ReadTeacherPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Teacher.Read")));
	option.AddPolicy("CRUDTeacherPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Teacher.Create.Read.Update.Delete")));
	option.AddPolicy("ReadSchedulePolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Schedule.Read")));
	option.AddPolicy("CRUDSchedulePolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Schedule.Create.Read.Update.Delete")));
	option.AddPolicy("ReadGradePolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Grade.Read")));
	option.AddPolicy("CreateGradePolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Grade.Create")));
	option.AddPolicy("UpdateGradePolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Grade.Update")));
	option.AddPolicy("ReadGradeTypePolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("GradeType.Read")));
	option.AddPolicy("CRUDGradeTypePolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("GradeType.Create.Read.Update.Delete")));
	option.AddPolicy("ReadStudentPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Student.Read")));
	option.AddPolicy("CRUDStudentPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Student.Create.Read.Update.Delete")));
	option.AddPolicy("CancelRegistrySubjectPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Student.Cancel")));
	option.AddPolicy("ReadUserPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("User.Read")));
	option.AddPolicy("CRUDPermissionPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Permission.Create.Read.Update.Delete")));
	option.AddPolicy("ReadSalaryPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Salary.Read")));
	option.AddPolicy("CRUDSalaryPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Salary.Create.Read.Update.Delete")));
	option.AddPolicy("ReadVacationPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Vacation.Read")));
	option.AddPolicy("CRUDVacationPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Vacation.Create.Read.Update.Delete")));



});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
	option.SaveToken = true;
	option.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateAudience = true,
		ValidateIssuer = true,
		ValidAudience = builder.Configuration["AppSettings:ValidAudience"],
		ValidIssuer = builder.Configuration["AppSettings:ValidIssuer"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
			builder.Configuration.GetSection("AppSettings:Token").Value!)),
		ClockSkew = TimeSpan.Zero
	};
});


var app = builder.Build();

app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors("Open");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(_ => { });
app.MapControllers();

app.Run();
