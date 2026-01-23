using MagicOnion;
using MagicOnion.Server;
using SearchService.Api.Infrastructure.Persistence;
using SearchService.Contracts.Interfaces;
using SearchService.Contracts.Requests;

namespace SearchService.Api.Services;

/// <summary>
///    Implements RPC service in the server project.
/// </summary>
public class SearchService : ServiceBase<ISearchService>, ISearchService
{
    private readonly SearchDbContext _context;

    public SearchService(SearchDbContext context)
    {
        _context = context;
    }

    public async UnaryResult<SearchResponse> SearchAsync(SearchRequest request)
    {
        var query =  _context.JobSearchDocuments
            .Where(j => j.SearchVector.Matches(EF.Functions.PlainToTsQuery("english", request.SearchTerm)));
        
        var data = await query
            .Select(j => j.Id)
            .ToListAsync();

        return new SearchResponse
        {
            Ids = data,
        };
    }
}