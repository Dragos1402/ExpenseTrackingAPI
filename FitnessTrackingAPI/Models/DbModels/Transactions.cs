namespace ExpenseTrackingAPI.Models.DbModels
{
    public class Transactions
    {
        public int transaction_id { get; set; }
        public decimal value { get; set; }
        public DateTime transaction_date { get; set; }
        public string? note { get; set; }
        public TransactionCategory category { get; set; }
        public TransactionType type { get; set; }
        public int? inserted_by { get; set; }
        public DateTime? inserted_at_date { get; set; }
        public int? modified_by { get; set; }
        public DateTime? modified_at_date { get; set; }
        public int? closed_by { get; set; }
        public DateTime? closed_at_date { get; set; }

    } 
}
