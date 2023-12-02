var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();

var app = builder.Build();

app.MapDefaultControllerRoute();
app.UseFileServer();
app.Run();
