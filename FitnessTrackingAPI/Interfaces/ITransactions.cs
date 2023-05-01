namespace ExpenseTrackingAPI.Interfaces
{
    public interface ITransactions
    {
        string GetTransactions();
        string GetTransactionById();
        string AddTransaction();
        string PutTransaction();

    }
}
