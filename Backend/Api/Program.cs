using Alti.Domain.Exceptions;
using IOC;
using Microsoft.EntityFrameworkCore;

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

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var error = context.Features
            .Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (error?.Error is KeyNotFoundException)
        {
            context.Response.StatusCode = 404;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Not Found",
                message = error.Error.Message
            });
            return;
        }

        if (error?.Error is DomainException)
        {
            context.Response.StatusCode = 422;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Business Rule Violation",
                message = error.Error.Message
            });
            return;
        }

        if (error?.Error is ArgumentException)
        {
            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Bad Request",
                message = error.Error.Message
            });
            return;
        }

        if (error?.Error is DbUpdateException dbEx)
        {
            context.Response.StatusCode = 409;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Conflict",
                message = "A record with the same key already exists.",
                detail = dbEx.InnerException?.Message ?? dbEx.Message
            });
            return;
        }

        if (error?.Error is InvalidOperationException invEx)
        {
            context.Response.StatusCode = 422;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Business Rule Violation",
                message = invEx.Message
            });
            return;
        }

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Internal Server Error",
            message = "An unexpected error occurred."
        });
    });
});

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