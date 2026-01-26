using JetBrains.Annotations;
using MagicOnion;
using SearchService.Contracts.Requests;
using SearchService.Contracts.Responses;

namespace SearchService.Contracts.Interfaces;

// <summary>
//      Defines .NET interface as a Server/Client IDL.
//      The interface is shared between server and client.
// </summary>
[PublicAPI]
public interface ISearchService : IService<ISearchService>
{
    UnaryResult<SearchResponse> SearchAsync(SearchRequest request);
}