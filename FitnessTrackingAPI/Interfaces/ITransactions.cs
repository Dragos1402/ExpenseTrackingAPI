using ExpenseTrackingAPI.Models.TransactionModels;

namespace ExpenseTrackingAPI.Interfaces
{
    public interface ITransactions
    {
        List<TransactionList> Transactions { get; set; }
        Transaction Transaction { get; set; }
        object ResultID { get; set; }
        string GetTransactions();
        string GetTransactionById(int id);
        string AddTransaction(AddTransaction addTransaction);
        string UpdateTransaction(UpdateTransaction updateTransaction);

    }
}
