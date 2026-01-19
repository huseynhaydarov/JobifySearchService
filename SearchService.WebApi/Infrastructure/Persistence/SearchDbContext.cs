using SearchService.Domain;

namespace SearchService.Infrastructure.Persistence;

public class SearchDbContext : DbContext
{
    public SearchDbContext(DbContextOptions<SearchDbContext> options)
        : base(options)
    {
    }

    public DbSet<JobSearchDocument> JobSearchDocuments => Set<JobSearchDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobSearchDocument>()
            .HasKey(x => x.Id);

        modelBuilder.Entity<JobSearchDocument>()
            .HasGeneratedTsVectorColumn(
                p => p.SearchVector,
                "english",
                p => new
                {
                    p.Title,
                    p.Description,
                    p.Requirements
                })
            .HasIndex(p => p.SearchVector)
            .HasMethod("GIN");

        base.OnModelCreating(modelBuilder);
    }
}