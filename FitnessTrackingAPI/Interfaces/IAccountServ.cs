using System.Threading.Tasks;
using ExpenseTrackingAPI.DbModels;
using ExpenseTrackingAPI.Models;

namespace ExpenseTrackingAPI.Interfaces
{
    public interface IAccountServ
    {
        int ResultID { get; set; }
        object Token { get; set; }
        string Register(AccountRegisteration account);
        string Login(AccountLogin accountLogin);
        string ResetPassword(string email);
        string UpdateAccount(UpdateAccount updateAccount, string token);
        string DeleteAccount(DeleteAccount deleteAccount, string token);
    }
}
