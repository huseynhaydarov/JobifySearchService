using System;
using System.Collections.Generic;
using MessagePack;

namespace SearchService.Contracts.Responses;

[MessagePackObject]
public record SearchResponse
{
    [Key(0)] public IReadOnlyList<Guid> Ids { get; init; }

}