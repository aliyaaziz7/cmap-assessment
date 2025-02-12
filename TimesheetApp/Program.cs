using Timesheet.Models;
using Microsoft.EntityFrameworkCore;
using TimesheetApp.Interfaces;
using TimesheetApp;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using TimesheetApp.Validators;
using FluentValidation;
using TimesheetApp.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TimesheetContext>(opt => 
{
    opt.UseInMemoryDatabase("InMemoryDb");
});

builder.Services.AddScoped<IValidator<TimesheetCreateModel>, TimesheetCreateModelValidator>();
builder.Services.AddScoped<ITimesheetRepository, TimesheetRepository>();

// If using Kestrel:
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// If using IIS:
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

DbInitialiser.InitDb(app);

app.Run();
