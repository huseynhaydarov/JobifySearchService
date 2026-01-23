using Jobify.Contracts.JobListings.Events;
using SearchService.Api.Domain;
using SearchService.Api.Infrastructure.Persistence;

namespace SearchService.Api.Infrastructure.Consumers;

public class JobListingDeletedConsumer : IConsumer<JobListingDeletedEvent>
{
    private readonly ILogger<JobListingDeletedConsumer> _logger;
    private readonly SearchDbContext _dbContext;

    public JobListingDeletedConsumer(ILogger<JobListingDeletedConsumer> logger, SearchDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<JobListingDeletedEvent> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("Consuming jobListing event data: {jobListingEvent}", context.Message);

        var document = new JobSearchDocument
        {
            Id = message.Id,
        };

        _dbContext.JobSearchDocuments.Remove(document);
        await _dbContext.SaveChangesAsync();
    }
}