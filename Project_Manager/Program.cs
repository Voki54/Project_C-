using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Data.DAO.Repository;
using Project_Manager.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ITeamUserRepository, TeamUserRepository>();


//TODO отредактировать по завершении отладки пользователей!
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<AppUser, IdentityRole/*<string>*//*<Guid>*/>(
	options => 
	{
		options.Password.RequiredUniqueChars = 0;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequiredLength = 3;
		options.Password.RequireLowercase = false;
		options.Password.RequireUppercase = false;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // ƒобавление аутентификации
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ProjectTasks}/{action=Index}/{id?}");

app.Run();
