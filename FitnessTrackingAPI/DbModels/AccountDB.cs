namespace ExpenseTrackingAPI.DbModels
{
    public class AccountDB
    {
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public int user_type { get; set; }
        public string user_status { get; set; }
        public DateTime expiration_date { get; set; }
        public string note { get; set; }
        public int? created_by { get; set; }
        public DateTime? created_at_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_at_date { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? deleted_at_date { get; set; }

    }
}
