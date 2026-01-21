using System;
using System.Collections.Generic;

namespace SearchService.Contracts.Responses;

public record SearchResponse(IReadOnlyList<Guid> Ids);