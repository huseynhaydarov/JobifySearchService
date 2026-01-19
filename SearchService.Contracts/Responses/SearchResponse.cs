namespace SearchService.Contracts.Responses;

public record SearchResponse(IReadOnlyList<Guid> Ids);