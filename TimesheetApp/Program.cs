using Timesheet.Models;
using Microsoft.EntityFrameworkCore;
using TimesheetApp.Interfaces;
using TimesheetApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TimesheetContext>(opt => 
{
    opt.UseInMemoryDatabase("InMemoryDb");
});

builder.Services.AddScoped<ITimesheetRepository, TimesheetRepository>();

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
