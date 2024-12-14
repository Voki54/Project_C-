using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Data.DAO.Repository;
using Project_Manager.Models;
using Project_Manager.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<IProjectUserRepository, ProjectUserRepository>()
                .AddScoped<IJoinProjectRequestRepository, JoinProjectRequestRepository>()
                .AddScoped<ProjectUserService>();

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

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
