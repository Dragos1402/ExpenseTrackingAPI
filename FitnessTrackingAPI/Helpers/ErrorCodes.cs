namespace ExpenseTrackingAPI.Helpers
{
    public class ErrorCodes
    {
        public const string SUCCESS = "000";
        public const string DATABASE_READING_ERROR = "001";
        public const string DATABASE_WRITING_ERROR = "002";
        public const string DATABASE_UPDATE_ERROR = "003";
        public const string NO_RESULTS = "004";
        public const string NO_ID_PROVIDED = "005";
    }
}
