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
        public const string UNAUTHORIZED = "006";
        public const string INVALID_EMAIL_OR_PASSWORD = "007";
        public const string ACCOUNT_NOT_FOUND = "008";
        public const string INVALID_TOKEN = "009";
        public const string INCORRECT_PASSWORD = "010";
    }
}
