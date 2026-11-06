using Microsoft.EntityFrameworkCore;
using PetCareHub.Database;
using PetCareHub.Database.Repositories;
using PetCareHub.Domain.Common;
using PetCareHub.Domain.Features.Pets;
using PetCareHub.Domain.Features.Reminders;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database - SQLite for easy local run (no SQL Server needed)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("PetCareHub") ?? "Data Source=petcarehub.db"));

// Repositories (Repository Pattern)
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IReminderRepository, ReminderRepository>();

// Domain services
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IReminderService, ReminderService>();

// Factory & Strategy
builder.Services.AddSingleton<PetFactory>();          // Singleton
builder.Services.AddSingleton<ReminderStrategyFactory>(); // Singleton

// Observer
builder.Services.AddSingleton<IEventDispatcher, InMemoryEventDispatcher>();

// Adapter + Circuit Breaker
builder.Services.AddSingleton<INotificationAdapter>(sp =>
{
    var fake = new FakeNotificationAdapter();
    return new CircuitBreakerNotificationAdapter(fake);
});

var app = builder.Build();

// Create database + seed sample data on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbInitializer.Seed(db);
}

// Decorator Pattern - simple logging middleware
app.Use(async (context, next) =>
{
    Console.WriteLine($"[Request] {context.Request.Method} {context.Request.Path}");
    await next();
    Console.WriteLine($"[Response] {context.Response.StatusCode}");
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
