using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Ordin.Application.DependencyInjection;
using Ordin.Application.Dispatchers;
using Ordin.Application.Interfaces;
using Ordin.Application.Services;
using Ordin.Infra.Contexts;
using Ordin.Infra.DependencyInjection;
using System.Reflection;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       "Host=localhost;Database=ordin;Username=postgres;Password=3011";

const string AllowAnyLocalhostPolicy = "_allowAnyLocalhostOriginPolicy";

builder.Services.AddDbContext<OrdinContext>(options =>
    options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

builder.Services.AddScoped<IDispatcher, Dispatcher>();
builder.Services.AddScoped<ICurrentUserService, UserService>();

builder.Services.AddApplicationHandlers();
builder.Services.AddInfrastructureServices();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ordin API", Version = "v1" });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddCors(options => 
    options.AddPolicy(AllowAnyLocalhostPolicy, 
    policy => policy.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordin API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseCors(AllowAnyLocalhostPolicy);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
