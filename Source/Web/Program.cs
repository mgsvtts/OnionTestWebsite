using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Repositories;
using Presentation;
using Services;
using Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc()
    .AddApplicationPart(typeof(AssemblyReference).Assembly);

var connectionString = "";
if (builder.Environment.EnvironmentName == "Docker")
{
    connectionString = "DockerDatabase";
}
else
{
    connectionString = "OrdinaryDatabase";
}

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(connectionString)));

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();

builder.Services.AddScoped<IServiceManager, ServiceManager>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();