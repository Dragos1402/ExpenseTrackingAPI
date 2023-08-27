using ExpenseTrackingAPI.Models;

namespace ExpenseTrackingAPI.Interfaces
{
    public interface ITransactions
    {
        List<TransactionList> Transactions { get; set; }
        Transaction Transaction { get; set; }
        object ResultID { get; set; }
        string GetTransactions(int id);
        string GetTransactionById(int transactionID, int accID);
        string AddTransaction(AddTransaction addTransaction, int accID);
        string UpdateTransaction(UpdateTransaction updateTransaction, int accID);

    }
}
