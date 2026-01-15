namespace SearchService.Application.Dtos;

public record SearchResponse(IReadOnlyList<Guid> Ids);