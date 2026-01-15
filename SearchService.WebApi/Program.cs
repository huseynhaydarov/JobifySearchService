var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SearchDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("jobListings/search", async (string searchTerm, SearchDbContext context) =>
{
    var query =  context.JobSearchDocuments
        .Where(j => j.SearchVector.Matches(EF.Functions.PlainToTsQuery("english", searchTerm)));
        
    var data = await query
        .Select(j => j.Id)
        .ToListAsync();

    var response = new SearchResponse(Ids:data);
    
    return response;
});

app.Run();