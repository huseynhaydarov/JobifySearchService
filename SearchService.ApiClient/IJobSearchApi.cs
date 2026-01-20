using Refit;
using SearchService.Contracts.Requests;
using SearchService.Contracts.Responses;

namespace SearchService.ApiClient;

public interface IJobSearchApi
{
    [Get(ApiEndpoints.JobListings.Search)]
    Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
}