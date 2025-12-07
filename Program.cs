using JasperFx;
using Marten;
using Wolverine;
using Wolverine.Http;
using Wolverine.Marten;
using FreightShipping;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Utils.GetConnectionString();

// Configure Marten with Wolverine integration
builder.Services.AddMarten(opts =>
    {
        opts.Connection(connectionString!);
        opts.AutoCreateSchemaObjects = AutoCreate.All; // Dev mode: create tables if missing
    })
    .UseLightweightSessions()
    .IntegrateWithWolverine();

// Add Wolverine with HTTP endpoints
builder.Host.UseWolverine(opts =>
{
    // Discover and register all HTTP endpoints
    opts.Discovery.IncludeAssembly(typeof(Program).Assembly);
    // Enable Transactional Middleware for Marten
    opts.Policies.AutoApplyTransactions();
});

// Add Wolverine HTTP
builder.Services.AddWolverineHttp();

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new() { Title = "Freight Shipping API", Version = "v1" }); });

var app = builder.Build();

// Configure Swagger UI - must be before MapWolverineEndpoints
app.UseSwagger();
app.UseSwaggerUI();

// Map Wolverine HTTP endpoints
app.MapWolverineEndpoints();

await app.RunAsync();