namespace ExpenseTrackingAPI.DbModels
{
    public class Token
    {
        public int tk_id { get; set; }
        public string tk_value { get; set; }
        public string tk_ip_requested { get; set; }
        public int tk_account_id { get; set; }
        public DateTime tk_expiration_date { get; set; }
        public int? inserted_by { get; set; }
        public DateTime? inserted_at_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_at_date { get; set; }
        public int? deleted_by { get; set; }
        public DateTime? deleted_at_date { get; set; }
    }
}
