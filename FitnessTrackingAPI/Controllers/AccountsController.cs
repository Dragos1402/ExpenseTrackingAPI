using System.Threading.Tasks;
using ExpenseTrackingAPI.DbModels;
using ExpenseTrackingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using ExpenseTrackingAPI.Models;
using ExpenseTrackingAPI.Services;
using Microsoft.AspNetCore.Authorization;
using ExpenseTrackingAPI.Helpers;
using Microsoft.AspNetCore.Http;
using System.Reflection;
using NuGet.Protocol;
using Serilog;

namespace ExpenseTrackingAPI.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountServ _accountService;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;


        public AccountController(IAccountServ accountService, Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor, TokenServ token)
        {
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] AccountRegisteration account)
        {
            string currentMethodName = MethodBase.GetCurrentMethod().Name;
            Response response = new Response();

            try
            {
                string result = _accountService.Register(account);
                if (result == ErrorCodes.SUCCESS)
                {
                    response.data = _accountService.ResultID;
                    response.SetResponse(result, currentMethodName);

                    Log.Information("Account: " + account.email + "registered succesfully!");
                    return Ok(response);
                }
                Log.Warning("Registration failed for account: " + account.email + ". Reason: " + result);
                response.SetResponse(result, currentMethodName);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected error occured during Registration process. Reason: " + ex.Message);
                response.SetResponse(ErrorCodes.DATABASE_WRITING_ERROR, currentMethodName, ex);
                return StatusCode(500, response);
            }
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] AccountLogin accountLogin)
        {
            Response response = new Response();

            try
            {
                string result = _accountService.Login(accountLogin);
                if (result == ErrorCodes.SUCCESS)
                {
                    response.data = _accountService.Token;
                    response.SetResponse(result, MethodBase.GetCurrentMethod().Name);

                    Log.Information("Login succeded for user: {Username}", accountLogin.email);
                    return Ok(response);
                }
                else if (result == ErrorCodes.ACCOUNT_NOT_FOUND)

                    Log.Warning("Bad Request for User Login");
                    response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                    return BadRequest(response);
            }
            catch (Exception ex)
            {
                Log.Error($"An error occured during login process for user : {accountLogin.email}. Reason:" + ex.Message);

                response.SetResponse(ErrorCodes.DATABASE_WRITING_ERROR, MethodBase.GetCurrentMethod().Name , ex);
                return StatusCode(500, response);
            }
        }

        [HttpPost("reset_password")]    // To be implemented
        public ActionResult ResetPassword()
        {
            Response response = new Response();

            try
            {
                string result = "";
                if (result == ErrorCodes.SUCCESS)
                {
                    
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(400, ex);  // To be updated!
            }
        }
        [HttpPost("delete_account")]
        public ActionResult DeleteAccount(DeleteAccount deleteAccount)
        {
            Response response = new Response();
            try
            {
                string result = _accountService.DeleteAccount(deleteAccount, _httpContextAccessor.HttpContext.Request.Headers["Token"]);
                if (result== ErrorCodes.SUCCESS)
                {
                    response.data = _accountService.ResultID;
                    response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                }
                Log.Information("Account: " + deleteAccount.email + " was succesfully deleted");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error("An unexpected error occured during deleting account process. Reason: " + ex.Message);
                response.SetResponse(ErrorCodes.DATABASE_WRITING_ERROR, MethodBase.GetCurrentMethod().Name, ex);
                return Ok(response);
            }
        }

        [HttpPost("update_account")]
        public ActionResult UpdateAccount(UpdateAccount updateAccount)
        {
            Response response = new Response();
            try
            {
                string result = _accountService.UpdateAccount(updateAccount, _httpContextAccessor.HttpContext.Request.Headers["Token"]);
                if (result == ErrorCodes.SUCCESS)
                {
                    response.data = _accountService.ResultID;
                    response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                }
                Log.Information("Account : " + updateAccount.email + " was succesfully updated");
                return Ok(response);
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected error occured when trying to update account: " + updateAccount.email + ". Reason: " + ex.Message);
                response.SetResponse(ErrorCodes.DATABASE_WRITING_ERROR, MethodBase.GetCurrentMethod().Name, ex);
                return Ok(response);
            }
        }
    }
}
