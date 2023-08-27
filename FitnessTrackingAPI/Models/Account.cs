namespace ExpenseTrackingAPI.Models
{
    public class AccountLogin
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class AccountRegisteration
    { 
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int user_type { get; set; }
        public string note { get; set; }
    }
    public class AccountToken
    {
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
    }
    public class DeleteAccount
    {
        public string email { get; set; }
        public string password { get; set; }
    }
    public class UpdateAccount : AccountRegisteration
    {
    }
}
