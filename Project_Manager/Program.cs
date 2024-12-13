using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_Manager.Data;
using Project_Manager.Data.DAO.Interfaces;
using Project_Manager.Data.DAO.Repository;
using Project_Manager.Events.Notification;
using Project_Manager.Events.Notification.EventHandlers;
using Project_Manager.Models;
using Project_Manager.Services;
using Project_Manager.Services.Interfaces;
using Project_Manager.StatesManagers;
using Project_Manager.StatesManagers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>()
				.AddScoped<IProjectUserRepository, ProjectUserRepository>()
				.AddScoped<IJoinProjectRequestRepository, JoinProjectRequestRepository>()
				.AddScoped<INotificationRepository, NotificationRepository>()
                .AddScoped<IProjectService, ProjectService>()
                .AddScoped<IProjectUserService, ProjectUserService>()
				.AddScoped<INotificationService, NotificationService>()
                .AddScoped<IJoinProjectService, JoinProjectService>()
                .AddScoped<IParticipantService, ParticipantService>()
                .AddScoped<IAccountService, AccountService>()
                .AddScoped<INotificationStatesManager, NotificationStatesManager>()
				.AddScoped<EventPublisher>()
				.AddScoped<NotificationEventHandler>();

//TODO отредактировать по завершении отладки пользователей!
builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<AppUser, IdentityRole>(
	options => 
	{
		options.Password.RequiredUniqueChars = 2;
		options.Password.RequireNonAlphanumeric = false;
		options.Password.RequiredLength = 3;
		options.Password.RequireLowercase = false;
		options.Password.RequireUppercase = false;
	})
	.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


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
    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapControllers();

app.Run();
