using Microsoft.EntityFrameworkCore;
using WebQuiz.Data;
using WebQuiz.Models.Repositories;
using WebQuiz.Models.RepositoryIntefraces;
using WebQuiz.Models.Service;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IOfferRepository, OfferRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IRulerRepository, RulerRepository>();
builder.Services.AddScoped<IService, Service>();

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
