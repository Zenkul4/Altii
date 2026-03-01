using Alti.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<HotelDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("Alti.Infrastructure")));


builder.Services.AddScoped<ICurrentUserService, DummyCurrentUserService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();

public class DummyCurrentUserService : ICurrentUserService
{
    public int? UsuarioId => 1;
}