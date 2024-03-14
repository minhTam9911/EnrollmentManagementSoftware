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
	option.AddPolicy("Open",policy=>policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
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
builder.Services.AddScoped<IScheduleService , ScheduleService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddSingleton<IAuthorizationHandler, IsPermissionWithRoleHandle>();
builder.Services.AddAuthorization(option =>
{
	option.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
	option.AddPolicy("ReadClassListPolicy",policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Read class list")));
	option.AddPolicy("ReadStudentListInClassPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Read the list of students in class")));
	option.AddPolicy("ReadSubjectListInClassPolicy", policy => policy.AddRequirements(new IsPermissionWithRoleRequirement("Read the list of subjects in class")));
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
{
	option.SaveToken = true;
	option.TokenValidationParameters = new TokenValidationParameters
	{
		ValidateIssuerSigningKey = true,
		ValidateAudience =true,
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
