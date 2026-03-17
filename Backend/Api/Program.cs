using IOC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new()
    {
        Title = "ALTI Hotel — Desktop API",
        Version = "v1",
        Description = "API para el sistema de gestión interno del hotel"
    });
});

builder.Services.AddAllServices(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DesktopPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", async (Persistence.Context.AppDbContext db) =>
{
    var canConnect = await db.Database.CanConnectAsync();
    return canConnect
        ? Results.Ok("Database connection successful.")
        : Results.Problem("Cannot connect to database.");
});

app.UseCors("DesktopPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();