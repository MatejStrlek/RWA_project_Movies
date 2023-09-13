using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RWA_MVC_project.Models;
using RWA_MVC_project.Repos;
using RWA_MVC_project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<RwaMoviesContext>(options =>
{
    //options.UseSqlServer(builder.Configuration.GetConnectionString("RwaMovies"));
    options.UseSqlServer("name=ConnectionStrings:DefaultConnection");
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IMailSender, MailSender>();
builder.Services.AddScoped<IEmailMessageRepo, EmailMessageRepo>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Videos/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Videos}/{action=Index}/{id?}");

app.Run();
