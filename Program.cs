using JasperFx;
using Marten;
using Wolverine;
using Wolverine.Http;
using Wolverine.Marten;
using FreightShipping;
using FreightShipping.EventSourcing.Views;
using JasperFx.Events.Daemon;
using JasperFx.Events.Projections;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Utils.GetConnectionString();

// Configure Marten with Wolverine integration
builder.Services.AddMarten(opts =>
    {
        opts.Connection(connectionString!);
        opts.AutoCreateSchemaObjects = AutoCreate.All; // Dev mode: create tables if missing
        
        opts.Projections.Add<DailyShipmentsProjection>(ProjectionLifecycle.Async);
        opts.Projections.Add<ShipmentViewProjection>(ProjectionLifecycle.Async);

    })
    .UseLightweightSessions()
    // Turn on the async daemon in "Solo" mode
    // there are other modes, but this is the simplest
    .AddAsyncDaemon(DaemonMode.Solo)
    .IntegrateWithWolverine()
    .PublishEventsToWolverine("freightshipping-events");
    ;

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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Freight Shipping API", Version = "v1" });

    // Use full namespace as part of schema ID to avoid naming conflicts
    c.CustomSchemaIds(type => type.FullName?.Replace("+", "."));
});

var app = builder.Build();

// Configure Swagger UI - must be before MapWolverineEndpoints
app.UseSwagger();
app.UseSwaggerUI();

// Map Wolverine HTTP endpoints
app.MapWolverineEndpoints();

await app.RunAsync();