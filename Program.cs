using CRMProductSystem.Data;
using CRMProductSystem.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Licensing;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSession();


// DB
builder.Services.AddScoped<DbConnection>();
builder.Services.AddSingleton<CRMProductSystem.Data.DbConnection>();


// SERVICES
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<FollowupService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderItemService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddSingleton<ReportService>();

var app = builder.Build();

// Syncfusion License
SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF1cXmhKYVJxWmFZfVhgd19FaVZTQWYuP1ZhSXxVdkZiWX9dc31XQ2dYWUB9XEA=");
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JGaF1cXmhKYVJxWmFZfVhgd19FaVZTQWYuP1ZhSXxVdkZiWX9dc31XQ2dYWUB9XEA=");

app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();