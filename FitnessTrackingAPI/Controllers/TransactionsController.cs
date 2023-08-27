using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackingAPI.DataContext;
using ExpenseTrackingAPI.DbModels;
using ExpenseTrackingAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using ExpenseTrackingAPI.Helpers;
using System.Reflection;
using ExpenseTrackingAPI.Models;
using IHttpContextAccessor = Microsoft.AspNetCore.Http.IHttpContextAccessor;
using Serilog;

/* ***************************** IMPLEMENT LOGGING ******************************** */

namespace ExpenseTrackingAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        readonly ITransactions _transactions;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _contextAccessor;
        public TransactionsController(ITransactions transactions, IHttpContextAccessor httpContextAccessor)
        {
            _transactions = transactions;
            _contextAccessor = httpContextAccessor;
        }

        [HttpGet("get_transactions")]
        public ActionResult GetTransactions()
        {
            Response response = new Response();
            int accID = Globals.CheckToken(_contextAccessor.HttpContext.Request.Headers["Token"].ToString());

            if (accID != 0)
            {
                try
                {
                    string result = _transactions.GetTransactions(accID);
                    if (result == ErrorCodes.SUCCESS)
                    {
                        response.data = _transactions.Transactions;
                        response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                        Log.Information($"TransactionsController called with success and got all data for method: {MethodBase.GetCurrentMethod().Name}");
                        return Ok(response);
                    }
                    else
                    {
                        response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                        Log.Information($"TransactionsController failed for method: {MethodBase.GetCurrentMethod().Name}");
                        return BadRequest(response);
                    }
                }
                catch (Exception ex)
                {
                    response.SetResponse(ErrorCodes.DATABASE_READING_ERROR, MethodBase.GetCurrentMethod().Name, ex);
                    Log.Error($"TransactionsController encountered an unexpected error for method: {MethodBase.GetCurrentMethod().Name}. Exception: {ex.Message}");
                    return BadRequest(response);
                }
            }
            else
            {
                response.SetResponse(ErrorCodes.UNAUTHORIZED, MethodBase.GetCurrentMethod().Name, new Exception("Unauthorized"));
                Log.Error($"TransactionsController failed due to unauthorized access for method: {MethodBase.GetCurrentMethod().Name}");
                return Unauthorized(response);
            }
        }


        [HttpGet("get_transaction/{id}")]
        public ActionResult GetTransactionByID(int id)
        {
            string result = "";
            Response response = new Response();
            int accID = Globals.CheckToken(_contextAccessor.HttpContext.Request.Headers["Token"].ToString());

            if (accID != 0)
            {
                try
                {
                     result = _transactions.GetTransactionById(id, accID);
                    if (result == ErrorCodes.SUCCESS)
                    {
                        response.data = _transactions.Transaction;
                        response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                        Log.Information($"TransactionsController called with success and got all data for method: {MethodBase.GetCurrentMethod().Name}");
                        return Ok(response);
                    }
                    else
                    {
                        response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                        Log.Error($"TransactionsController failed for method: {MethodBase.GetCurrentMethod().Name}");
                        return BadRequest(response);
                    }
                }
                catch (Exception ex)
                {
                    response.SetResponse(ErrorCodes.DATABASE_READING_ERROR, MethodBase.GetCurrentMethod().Name, ex);
                    Log.Error($"TransactionsController encountered an unexpected error for method: {MethodBase.GetCurrentMethod().Name}. Exception: {ex.Message}");
                    return BadRequest(response);
                }
            }
            else
            {
                response.SetResponse(ErrorCodes.UNAUTHORIZED, MethodBase.GetCurrentMethod().Name, new Exception("Unauthorized"));
                Log.Error($"TransactionsController failed due to unauthorized access for method: {MethodBase.GetCurrentMethod().Name}");
                return Unauthorized(response);
            }
        }

        [HttpPost("add_transaction")]
        public ActionResult AddTransaction([FromBody] AddTransaction addTransaction)
        {
            string result = "";
            Response response = new Response();
            int accID = Globals.CheckToken(_contextAccessor.HttpContext.Request.Headers["Token"].ToString());

            if (accID != 0)
            {
                try
                {
                    result = _transactions.AddTransaction(addTransaction, accID);
                    if (result == ErrorCodes.SUCCESS)
                    {
                        response.data = _transactions.ResultID;
                        response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                    }
                }
                catch (Exception ex)
                {
                    response.SetResponse(result, MethodBase.GetCurrentMethod().Name, ex);
                    return BadRequest(response);
                }
                return Ok(response);
            }
            else
            {
                result = ErrorCodes.UNAUTHORIZED;
                response.SetResponse(result, MethodBase.GetCurrentMethod().Name, new Exception("Unauthorized"));
                return Unauthorized(response);
            }
        }

        [HttpPost("update_Transaction")]
        public ActionResult UpdateTransaction([FromBody] UpdateTransaction updateTransaction)
        {
            string result = "";
            Response response = new Response();
            int accID = Globals.CheckToken(_contextAccessor.HttpContext.Request.Headers["Token"].ToString());

            if (accID != 0)
            {
                try
                {
                    result = _transactions.UpdateTransaction(updateTransaction, accID);
                    if (result == ErrorCodes.SUCCESS)
                    {
                        response.data = _transactions.ResultID;
                        response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                    }
                }
                catch (Exception ex)
                {
                    response.SetResponse(result, MethodBase.GetCurrentMethod().Name, ex);
                    return BadRequest(response);
                }
                return Ok(response);
            }
            else
            {
                result = ErrorCodes.UNAUTHORIZED;
                response.SetResponse(result, MethodBase.GetCurrentMethod().Name, new Exception("Unauthorized"));
                return Unauthorized(response);
            }
        }
    }
}
