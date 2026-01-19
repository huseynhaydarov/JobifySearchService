using Refit;
using SearchService.Contracts.Responses;

namespace SearchService.ApiClient;

public interface IJobSearchApi
{
    [Get(ApiEndpoints.JobListings.Search)]
    Task<SearchResponse> SearchAsync(string searchTerm,
        CancellationToken cancellationToken);
}