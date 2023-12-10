using FruitStore.Repositories;
using GrooveHub.Models.Entities;
using GrooveHub.Repositories;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<Repository<Album>>();
builder.Services.AddTransient<Repository<Cancion>>();
builder.Services.AddTransient<CancionesRepository>();

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
app.UseFileServer();
app.MapDefaultControllerRoute();
app.Run();
