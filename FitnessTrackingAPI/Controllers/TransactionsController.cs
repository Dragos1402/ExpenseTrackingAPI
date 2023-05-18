using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseTrackingAPI.DataContext;
using ExpenseTrackingAPI.Models.DbModels;
using ExpenseTrackingAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using ExpenseTrackingAPI.Helpers;
using ExpenseTrackingAPI.Models.TransactionModels;
using System.Reflection;

namespace ExpenseTrackingAPI.Controllers
{
    // DE PASTRAT

    // Select [category_name] from [TransactionCategories] where [TransactionCategories].category_id=(SELECT [category_id] FROM [Transaction] where [value]=213) 

    [Route("api/transaction")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        readonly ITransactions _transactions;
        private readonly IHttpContextAccessor _contextAccessor;
        public TransactionsController(ITransactions transactions, IHttpContextAccessor httpContextAccessor)
        {
            _transactions = transactions;
            _contextAccessor = httpContextAccessor;
        }
        [HttpGet("get_transactions")]
        public ActionResult GetTransactions()
        {
            string result = "";
            Response response = new Response();

            try
            {
                result = _transactions.GetTransactions();
                if (result == ErrorCodes.SUCCESS)
                {
                    response.data = _transactions.Transactions;
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

        //[Authorize]
        [HttpGet("get_transaction/{id}")]
        public ActionResult GetTransactionByID(int id)
        {
            string result = "";
            Response response= new Response();

            try
            {
                result = _transactions.GetTransactionById(id);
                if (result == ErrorCodes.SUCCESS)
                {
                    response.data = _transactions.Transaction;
                    response.SetResponse(result, MethodBase.GetCurrentMethod().Name);
                }
            }
            catch (Exception ex)
            {
                response.SetResponse(result, MethodBase.GetCurrentMethod().Name, ex);
                return BadRequest(response);
                //Log.WriteError("GetTransactionByID", "GetTransactionByID(Controller) had an error", ex);
            }
            return Ok(response);
        }
        [HttpPost("add_transaction")]
        public ActionResult AddTransaction([FromBody] AddTransaction addTransaction)
        {
            string result = "";
            Response response = new Response();
            try
            {
                result = _transactions.AddTransaction(addTransaction);
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
        [HttpPost("update_Transaction")]
        public ActionResult UpdateTransaction([FromBody] UpdateTransaction updateTransaction)
        {
            string result = "";
            Response response = new Response();
            try
            {
                result = _transactions.UpdateTransaction(updateTransaction);
                if (result == ErrorCodes.SUCCESS)
                {
                    response.data= _transactions.ResultID;
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
    }
}
