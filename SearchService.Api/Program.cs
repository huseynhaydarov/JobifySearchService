using SearchService.Api.Infrastructure.Consumers;
using SearchService.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SearchDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMagicOnion();

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<JobListingCreatedConsumer>();
    options.AddConsumer<JobListingDeletedConsumer>();
    options.AddConsumer<JobListingUpdatedConsumer>();
    
    options.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"];
        
        cfg.Host(host, h =>
        {
            h.Username("guest"); 
            h.Password("guest");
        });
        
        cfg.ReceiveEndpoint("created-jobListing-queue",e =>
        {
            e.ConfigureConsumer<JobListingCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("deleted-jobListing-queue",e =>
        {
            e.ConfigureConsumer<JobListingDeletedConsumer>(context);
        });

        cfg.ReceiveEndpoint("updated-jobListing-queue", e =>
        {
            e.ConfigureConsumer<JobListingUpdatedConsumer>(context);
        });
        
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<SearchDbContext>();
        context.Database.Migrate();
        app.Logger.LogInformation("Database migrations applied successfully");
    }
    catch (Exception ex)
    {
        app.Logger.LogError(ex, "An error occurred while migrating the database");
        throw;
    }
}

app.MapMagicOnionService();

app.Run();