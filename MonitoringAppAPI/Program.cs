using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MonitoringAppAPI.Formatters;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using MonitoringAppAPI.Data;
using MonitoringAppAPI.Services; // Assuming you'll create this namespace for the ProtobufInputFormatter

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new ProtobufInputFormatter());
});

// Configure Kestrel to handle large payloads
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = 52428800; // 50MB
});

// Redis configuration
var redisConnectionString = builder.Configuration.GetSection("Redis")["ConnectionString"];
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnectionString));

//builder.Services.AddScoped<IRedisService, RedisService>();
builder.Services.AddSingleton<RedisService>();


// Add databasecontext
builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add CORS if necessary
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS if necessary
app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
