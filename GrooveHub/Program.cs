using FruitStore.Repositories;
using GrooveHub.Models.Entities;
using GrooveHub.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Repository<Album>>();
builder.Services.AddTransient<Repository<Cancion>>();
builder.Services.AddTransient<CancionesRepository>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(
    x =>
    {
        x.AccessDeniedPath = "/Home/Denied";
        x.LoginPath = "/Home/Login";
        x.LogoutPath = "/Home/Logout";
        x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        x.Cookie.Name = "ghcookie";
    });
builder.Services.AddDbContext<GroovehubContext>
(
    x => x.UseMySql("server=websitos256.com;password=0&f42sO2e;user=websitos_groovehub;database=websitos_groovehub", 
    Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.29-mysql"))
);

builder.Services.AddMvc();

var app = builder.Build();
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);
app.UseAuthentication();
app.UseAuthorization();
app.UseFileServer();
app.MapDefaultControllerRoute();
app.Run();
