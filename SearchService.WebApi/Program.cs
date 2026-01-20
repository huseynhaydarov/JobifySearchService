var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SearchDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

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

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("api/jobListings/search", async (string searchTerm, SearchDbContext context) =>
{
    var query =  context.JobSearchDocuments
        .Where(j => j.SearchVector.Matches(EF.Functions.PlainToTsQuery("english", searchTerm)));
        
    var data = await query
        .Select(j => j.Id)
        .ToListAsync();

    var response = new SearchResponse(Ids:data);
    
    return response;
});

app.Run();