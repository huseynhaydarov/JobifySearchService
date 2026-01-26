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
        cfg.Host("rabbitmq://localhost", h =>
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

app.MapMagicOnionService();

app.Run();