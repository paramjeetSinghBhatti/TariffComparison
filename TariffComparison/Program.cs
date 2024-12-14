using Serilog;
using TariffComparison.Factory;
using TariffComparison.Providers;
using TariffComparison.Repository;
using TariffComparison.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ITariffProvider,TariffProvider>();
builder.Services.AddSingleton<ITariffCalculatorFactory,TariffCalculatorFactory>();
builder.Services.AddSingleton<ITariffCalculatorRegistry, TariffCalculatorRegistry>();
builder.Services.AddScoped<ITariffComparisonRepository,TariffComparisonRepository>();

builder.Services.AddControllers();
builder.Services.AddMemoryCache();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Setup Serilog to log to a file
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/myapp-.log", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

// Use Serilog as the logging provider
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
