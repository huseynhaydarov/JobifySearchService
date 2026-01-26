using Jobify.Contracts.JobListings.Events;
using SearchService.Api.Domain;
using SearchService.Api.Infrastructure.Persistence;

namespace SearchService.Api.Infrastructure.Consumers;

public class JobListingCreatedConsumer : IConsumer<JobListingCreatedEvent>
{
    private readonly ILogger<JobListingCreatedConsumer> _logger;
    private readonly SearchDbContext _dbContext;

    public JobListingCreatedConsumer(ILogger<JobListingCreatedConsumer> logger, SearchDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    public async Task Consume(ConsumeContext<JobListingCreatedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("Consuming jobListing event data: {jobListingEvent}", context.Message);

        var document = new JobSearchDocument
        {
            Id = message.Id,
            Title = message.Name,
            Description = message.Description,
            Requirements = message.Requirements,
            PostedAt = message.PostedAt
        };

        _dbContext.JobSearchDocuments.Add(document);
        await _dbContext.SaveChangesAsync();
    }
}