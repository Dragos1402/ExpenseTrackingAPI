using ExpenseTrackingAPI.DbModels;

namespace ExpenseTrackingAPI.Models
{
    public class Transaction
    {
        public int transaction_id { get; set; }
        public decimal value { get; set; }
        public DateTime transaction_date { get; set; }
        public string? note { get; set; }

    }
    public class TransactionList
    {
        public int transaction_id { get; set; }
        public decimal value { get; set; }
        public DateTime transaction_date { get; set; }
        public string? note { get; set; }
        public TransactionCategory category { get; set; }
        public TransactionType type { get; set; }

    }
    public class AddTransaction
    {
        public decimal value { get; set; }
        public DateTime transaction_date { get; set; }
        public string? note { get; set; }
        public int category_id { get; set; }
        public int type_id { get; set; }
    }
    public class UpdateTransaction : AddTransaction
    {
        public int transaction_id { get; set; }
    }
}
