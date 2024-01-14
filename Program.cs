using Bookfy.Publishers.Api.Adapters;
using Bookfy.Publishers.Api.Ports;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration
        .GetSection(nameof(MongoDbSettings))
);

builder.Services.AddSingleton<IMongoClient>(new MongoClient(
    builder.Configuration
        .GetSection(nameof(MongoDbSettings))
        .GetValue<string>("ConnectionString")
));


builder.Services.AddScoped<IPublisherRepository, PublisherMongoDb>();
builder.Services.AddScoped<IPublisherUseCase, PublisherService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGroup("/v1")
    .MapPublisherRoutes();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
