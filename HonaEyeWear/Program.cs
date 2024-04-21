using HonaEyeWear.Data;
using HonaEyeWear.Models;
using HonaEyeWear.Repositories;
using HonaEyeWear.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireUppercase = false;          //không cần chữ in hoa
        options.Password.RequiredLength = 8;                //pass ít nhất 8 kí tự
        options.Password.RequireNonAlphanumeric = false;    //không cần có ký tự chữ cái hoặc số.
        options.Password.RequireLowercase = false;          //không yêu cầu chữ thường
    })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddTransient<ApplicationDbContext>();
//khai báo repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

//khai báo các service
builder.Services.AddScoped<IPropertyService, PropertyService>();
builder.Services.AddScoped<IPropertyValueService, PropertyValueService>();









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
