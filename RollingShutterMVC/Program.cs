using Microsoft.EntityFrameworkCore;
using RollingShutterProject.Data;
using RollingShutterProject.Interfaces;
using RollingShutterProject.Services;
using RollingShutterProject.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMqttService, MqttService>();




// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();



builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
    options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; 
    options.Cookie.SameSite = SameSiteMode.Lax; 
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
