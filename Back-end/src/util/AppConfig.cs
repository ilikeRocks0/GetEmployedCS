public sealed class AppConfig
{
    public string DBEnvConnectionString { get; set; } = "";

    public static class FilterKeys
    {
        public const string USERID = "UserId";
        public const string SEARCH_TERM = "searchTerm";
        public const string LANGUAGES = "languages";
        public const string POSITION_TYPES = "positionTypes";
        public const string EMPLOYMENT_TYPES = "employmentTypes";

        public const string PAGE_NUMBER = "pageNumber";
    }

    public const int ITEMS_PER_PAGE = 10;
}