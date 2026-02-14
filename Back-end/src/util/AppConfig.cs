public sealed class AppConfig
{
    public string DBEnvConnectionString { get; set; } = "";

    public static class FilterKeys
    {
        public const string SeekerId = "seekerId";
        public const string SearchTerm = "searchTerm";
        public const string Languages = "languages";
        public const string PositionTypes = "positionTypes";
        public const string EmploymentTypes = "employmentTypes";
    }
}