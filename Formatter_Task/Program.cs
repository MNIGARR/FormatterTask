using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Formatter_Task.Data;
using Formatter_Task.Formatters;
using Formatter_Task.MiddleWares;
using Formatter_Task.Repositories.Abstract;
using Formatter_Task.Repositories.Concrete;
using Formatter_Task.Services.Abstract;
using Formatter_Task.Services.Concrete;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new TextCsvInputFormatter());
    options.OutputFormatters.Insert(0, new TextCsvOutputFormatter());
    options.InputFormatters.Insert(0, new VcardInputFormatter());
    options.OutputFormatters.Insert(0, new VCardOutputFormatter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();



var connection = builder.Configuration.GetConnectionString("myconn");
builder.Services.AddDbContext<StudentDbContext>(opt =>
{
    opt.UseSqlServer(connection);
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
    options => {
        options.LoginPath = "/Account/SignIn";
        options.LogoutPath = "/Account/SignOut";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<AuthenticationMiddleware>();
app.UseAuthorization();

app.MapControllers();

app.Run();
