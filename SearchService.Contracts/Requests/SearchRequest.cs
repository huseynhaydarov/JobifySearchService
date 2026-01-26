using MessagePack;

namespace SearchService.Contracts.Requests;

[MessagePackObject]
public record SearchRequest
{
    [Key(0)] 
    public string SearchTerm { get; set; }
}