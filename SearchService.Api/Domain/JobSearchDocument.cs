using System.ComponentModel.DataAnnotations.Schema;
using NpgsqlTypes;

namespace SearchService.Api.Domain;

public class JobSearchDocument
{
    public Guid Id { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Requirements { get; set; }

    public DateTimeOffset PostedAt { get; set; }

    [Column(TypeName = "tsvector")]
    public NpgsqlTsVector SearchVector { get; set; } 
}