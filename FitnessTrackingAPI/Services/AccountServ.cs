using System.Threading.Tasks;
using ExpenseTrackingAPI.DataContext;
using ExpenseTrackingAPI.DbModels;
using ExpenseTrackingAPI.Interfaces;
using BCrypt.Net;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackingAPI.Helpers;
using ExpenseTrackingAPI.Models;
using Serilog;

namespace ExpenseTrackingAPI.Services
{
    public class AccountServ : IAccountServ
    {
        private readonly ExpenseContext _context;
        private readonly TokenServ _token;
        public AccountServ(IConfiguration config, TokenServ token)
        {
            _context = new(config);
            _token = token;
        }
        public int ResultID { get; set; }
        public object Token { get; set; }
        public string Register(Models.AccountRegisteration account)
        {
            try
            {
                // Hash the password before storing it in the database
                account.password = BCrypt.Net.BCrypt.HashPassword(account.password);

                // Save the account to the database
                _context.Accounts.Add(new AccountDB
                {
                    first_name = account.first_name,
                    last_name = account.last_name,
                    email = account.email,
                    user_name = account.user_name,
                    password = account.password,
                    user_type = account.user_type,
                    user_status = "A",
                    expiration_date = DateTime.UtcNow.AddMonths(12),
                    note = account.note,
                    created_at_date = null,
                    created_by = null,
                    modified_at_date = null,
                    modified_by = null,
                    deleted_at_date = null,
                    deleted_by = null,
                });
                _context.SaveChanges();

                Log.Information("Account successfully registered!");
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An unexpected error occurred during the registration process. Reason: {ErrorMessage}", ex.Message);
                return ErrorCodes.DATABASE_WRITING_ERROR;
            }
        }


        public string Login(AccountLogin accountLogin)
        {
            try
            {
                var account = _context.Accounts.FirstOrDefault(u => u.email == accountLogin.email);

                if (account != null)
                {
                    bool isSamePassword = BCrypt.Net.BCrypt.Verify(accountLogin.password, account.password);

                    if (isSamePassword)
                    {
                        this.Token = _token.GenerateToken(accountLogin);
                        DateTime tokenExpirationDate = DateTime.Now.AddDays(1);

                        var token = new Token
                        {
                            tk_value = this.Token.ToString(),
                            tk_ip_requested = "123.01.10",  // To be implemented! Necessary to receive from the FrontEnd
                            tk_account_id = (int)account.user_id,
                            tk_expiration_date = tokenExpirationDate,
                            inserted_by = account.user_id,
                            inserted_at_date = DateTime.Now
                        };

                        _context.Tokens.Add(token);
                        _context.SaveChanges();

                        Log.Information("Login was called successfully.");
                        return ErrorCodes.SUCCESS;
                    }
                }
                else
                    Log.Warning("Bad credentials. No user was found with email: " + accountLogin.email);
                    return ErrorCodes.ACCOUNT_NOT_FOUND;
            }
            catch (Exception ex)
            {
                Log.Error("An unexpected error occurred during login. Exception: " + ex.Message);
                return ErrorCodes.INVALID_EMAIL_OR_PASSWORD;
            }
        }



        public string ResetPassword(string email)
        {

            try
            {
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string DeleteAccount(DeleteAccount deleteAccount, string token)
        {
            try
            {
                // Verify the password first
                var account = _context.Accounts.FirstOrDefault(a => a.email == deleteAccount.email);
                bool isSamePassword = account != null && BCrypt.Net.BCrypt.Verify(deleteAccount.password, account.password);

                if (!isSamePassword)
                {
                    return ErrorCodes.INCORRECT_PASSWORD;
                }

                // Retrieve the account with the matching email and token
                account = (from a in _context.Accounts
                           join t in _context.Tokens on a.user_id equals t.tk_account_id
                           where a.email == deleteAccount.email && t.tk_value == token
                           select a).FirstOrDefault();

                if (account != null)
                {
                    // Set the deleted_by and deleted_at_date properties
                    account.deleted_by = account.user_id;
                    account.deleted_at_date = DateTime.Now;
                    account.user_status = "N";
                    _context.SaveChanges();
                }
                else
                {
                    Log.Warning("Bad credentials. No user was found with email: " + deleteAccount.email);
                    return ErrorCodes.ACCOUNT_NOT_FOUND;
                }
                Log.Information("Account deleted succesfully for User ID : " + account.user_id);
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                // Handle specific database update errors here
                Log.Error("An unexpected error occured during delete account process. Reason: " + ex.Message);
                return ErrorCodes.DATABASE_UPDATE_ERROR;
            }
        }

        public string UpdateAccount(UpdateAccount updateAccount, string token)
        {
            try
            {
                // Verify the password first
                var account = _context.Accounts.FirstOrDefault(a => a.email == updateAccount.email);

                // Retrieve the account with the matching email and token
                account = (from a in _context.Accounts
                           join t in _context.Tokens on a.user_id equals t.tk_account_id
                           where t.tk_value == token
                           select a).FirstOrDefault();

                if (account != null)
                {
                    // Set the deleted_by and deleted_at_date properties

                    account.first_name = updateAccount.first_name;
                    account.last_name = updateAccount.last_name;
                    account.email = updateAccount.email;
                    account.user_name = updateAccount.user_name;
                    account.password = updateAccount.password;
                    account.user_type = updateAccount.user_type;
                    account.note = updateAccount.note;
                    account.modified_at_date = DateTime.Now;
                    account.modified_by = account.user_id;

                    _context.SaveChanges();
                }
                else
                {
                    Log.Warning("Bad credentials. No user was found with email: " + account.email);
                    return ErrorCodes.ACCOUNT_NOT_FOUND;
                }

                Log.Information("Update account was called succesfully for user id: " + account.user_id);
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                // Handle specific database update errors here
                Log.Error("Unexpected error occured when updating account. Reason: " + ex.Message);
                return ErrorCodes.DATABASE_UPDATE_ERROR;
            }
        }
    }
}
