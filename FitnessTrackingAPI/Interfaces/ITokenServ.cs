using ExpenseTrackingAPI.DbModels;

namespace ExpenseTrackingAPI.Interfaces
{
    public interface IHttpContextAccessor
    {
        Task<string> GenerateToken(AccountDB account);
        Task<AccountDB> GetUser(string email, string password);
    }
}
