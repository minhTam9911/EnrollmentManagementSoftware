using EnrollmentManagementSoftware.Converters;
using EnrollmentManagementSoftware.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

var connectString = builder.Configuration["Connection:DefaultString"];
builder.Services.AddDbContext<DatabaseContext>(option => option.UseLazyLoadingProxies().UseSqlServer(connectString), ServiceLifetime.Singleton);
builder.Services.AddControllers();
builder.Services.AddControllers().AddJsonOptions(option =>
{
	option.JsonSerializerOptions.Converters.Add(new DateOnlyConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
var app = builder.Build();

app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseCors();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(_ => { });
app.MapControllers();

app.Run();
