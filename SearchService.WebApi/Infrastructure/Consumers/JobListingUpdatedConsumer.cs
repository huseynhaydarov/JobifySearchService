using Jobify.Contracts.JobListings.IntegrationEvents;
using SearchService.Domain;

namespace SearchService.Infrastructure.Consumers;

public class JobListingUpdatedConsumer : IConsumer<JobListingUpdated>
{
    private readonly ILogger<JobListingUpdatedConsumer> _logger;
    private readonly SearchDbContext _dbContext;
    
    public async Task Consume(ConsumeContext<JobListingUpdated> context)
    {
        var message = context.Message;
        
        _logger.LogInformation("Consuming jobListing event data: {jobListingEvent}", context.Message);

        var document = new JobSearchDocument
        {
            Id = message.Id,
            Title = message.Name,
            Description = message.Description,
            Requirements =  message.Requirements
        };

        _dbContext.JobSearchDocuments.Update(document);
        await _dbContext.SaveChangesAsync();
    }
}