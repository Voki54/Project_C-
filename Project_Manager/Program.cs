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

builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<AppUser, IdentityRole>(
	options => 
	{
        options.Password.RequiredUniqueChars = 4;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireDigit = true;
        options.User.RequireUniqueEmail = true;
        options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyz" +
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            "0123456789-_" + 
            "àáâãäå¸æçèéêëìíîïğñòóôõö÷øùûışÿ" +
            "ÀÁÂÃÄÅ¨ÆÇÈÉÊËÌÍÎÏĞÑÒÓÔÕÖ×ØÙÛİŞß";
    })
	.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.AddRazorPages();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();


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
