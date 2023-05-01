namespace ExpenseTrackingAPI.Models.DbModels
{
    public class Transactions
    {
        public int transaction_id { get; set; }
        public decimal value { get; set; }
        public string category { get; set; }
        public DateTime transaction_date { get; set; }
        public string? note { get; set; }

    } 
}
