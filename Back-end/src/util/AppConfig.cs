namespace Back_end.Util;
public sealed class AppConfig
{
    public static class FilterKeys
    {
        public const string USERID = "UserId";
        public const string JOBID = "JobId";
        public const string SEARCH_TERM = "searchTerm";
        public const string LANGUAGES = "languages";
        public const string POSITION_TYPES = "positionTypes";
        public const string EMPLOYMENT_TYPES = "employmentTypes";
        public const string PAGE_NUMBER = "pageNumber";
        public const string EMPLOYER = "employer";
    }

    public const int ITEMS_PER_PAGE = 10;

    //the amount of quiz item ids loaded in at a time from the database
    public const int QUIZ_ITEM_AMOUNT = 500;

    public const string DB_ENV_KEY = "DB_CONNECTION_STRING";
}