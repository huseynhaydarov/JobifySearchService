namespace SearchService.ApiClient;

public static class ApiEndpoints
{
    public const string Base = "/api";

    public static class JobListings
    {
        public const string Search = $"{Base}/jobListings/search";
    }
}