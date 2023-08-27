using ExpenseTrackingAPI.Interfaces;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Transactions;
using ExpenseTrackingAPI.DbModels;
using System.Data;
using ExpenseTrackingAPI.DataContext;
using ExpenseTrackingAPI.Helpers;
using ExpenseTrackingAPI.Models;
using NuGet.Common;
using Newtonsoft.Json.Linq;
using Serilog;

namespace ExpenseTrackingAPI.Services
{
    public class TransactionsServ : ITransactions
    {
        public List<TransactionList> Transactions { get; set; }
        public Models.Transaction Transaction { get; set; }
        public AccountDB Account { get; set; }
        public object ResultID { get; set; }

        private readonly ExpenseContext _context;

        public TransactionsServ(IConfiguration config)
        {
            _context = new(config);
        }

        public string GetTransactions(int accID)
        {
            Transactions = new List<TransactionList>();

            try
            {
                Transactions = (from d in _context.Transactions
                                where d.user_id == accID
                                      select new Models.TransactionList
                                      {
                                          transaction_id = d.transaction_id,
                                          value = d.value,
                                          transaction_date = d.transaction_date,
                                          note = d.note,
                                          category = d.category,
                                          type = d.type
                                      }).ToList();

                Log.Information("GetTransactions was called with success and got all the data");
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                Log.Error("GetTransactions got an unexpected error:" + ex.Message);
                return ErrorCodes.DATABASE_READING_ERROR;
            }
        }

        public string GetTransactionById(int transactionID, int accID)
        {
            
            this.Transaction = new Models.Transaction();
            this.Account = new AccountDB();
            try
            {
                if (accID==null)
                {
                    return ErrorCodes.NO_ID_PROVIDED;
                }
                else
                {

                    Transaction = (from d in _context.Transactions
                                   where d.transaction_id == transactionID
                                   where d.user_id == accID
                                   select new Models.Transaction
                                   {
                                       transaction_id = d.transaction_id,
                                       value = d.value,
                                       transaction_date = d.transaction_date,
                                       note = d.note,

                                   }).FirstOrDefault();

                    Log.Information("GetTransactionById was called with success and got all the requested data");
                    return ErrorCodes.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                Log.Information("GetTransactionById got an unexpected error: " + ex.Message);
                return ErrorCodes.DATABASE_READING_ERROR;
            }
        }

        public string AddTransaction(AddTransaction addTransaction, int accID)
        {
            try
            {
                var category = _context.TransactionCategories.FirstOrDefault(c => c.category_id == addTransaction.category_id);
                var type = _context.TransactionTypes.FirstOrDefault(t => t.type_id == addTransaction.type_id);

                var newTransaction = new DbModels.Transactions
                {
                    value = addTransaction.value,
                    transaction_date = addTransaction.transaction_date,
                    note = addTransaction.note,
                    category = category,
                    type = type,
                    user_id = accID,
                    created_by = accID,
                    created_at_date = DateTime.Now,
                };
                _context.Transactions.Add(newTransaction);
                _context.SaveChanges();

                this.ResultID = newTransaction.transaction_id;

                Log.Information("Transaction added succesfully for AccountID: " + accID);
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                Log.Error("An unexpected error occured during add transaction process. Reason: " + ex.Message);
                return ErrorCodes.DATABASE_WRITING_ERROR;
            }
        }

        public string UpdateTransaction(UpdateTransaction updateTransaction, int accID)
        {
            try
            {
                var transaction = _context.Transactions.FirstOrDefault(d => d.transaction_id == updateTransaction.transaction_id);

                if (transaction != null)
                {
                    var category = _context.TransactionCategories.FirstOrDefault(c => c.category_id == updateTransaction.category_id);
                    var type = _context.TransactionTypes.FirstOrDefault(t => t.type_id == updateTransaction.type_id);

                    transaction.value = updateTransaction.value;
                    transaction.transaction_date = updateTransaction.transaction_date;
                    transaction.note = updateTransaction.note;
                    transaction.category = category;
                    transaction.type = type;
                    transaction.user_id = accID;
                    transaction.modified_by = 2;
                    transaction.modified_at_date = DateTime.Now;

                    _context.SaveChanges();
                }

                this.ResultID = updateTransaction.transaction_id;

                Log.Information("Transaction succesfully updated for User: " + accID + ", for Transaction ID: " + updateTransaction.transaction_id);
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                Log.Error("Unexpected error occured for updating transaction. Reason: ", ex.Message);
                return ErrorCodes.DATABASE_UPDATE_ERROR;
            }
        }


        //string currentMethodName = MethodBase.GetCurrentMethod().Name;
        //try
        //{
        //    using (var conn = new SqlConnection(config.GetConnectionString("DefaultConnection")))
        //    {
        //        SqlCommand sqlCommand = new SqlCommand();
        //        sqlCommand.CommandText = sql;

        //        using (var reader = sqlCommand.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //            //   Transaction transaction = new Models.Transaction();

        //            //    Transaction transaction1 = new Models.Transaction({

        //            //        transaction_id = (int)reader.GetValue("transaction_id"),
        //            //        value = (decimal)reader.GetValue("value"),
        //            //        note = (string)reader.GetValue("note"),
        //            //        transaction_date=(DateTime)reader.GetValue("date"),
        //            //});

        //            //    //transaction.transaction_id = (int)reader.GetValue("transaction_id");
        //            //    //transaction.value=(decimal)reader.GetValue("value");
        //            //    //transaction.transaction_date=(DateTime)reader.GetValue ("date");
        //            //    //transaction.note = (string)reader.GetValue("note");
        //            //    Transactions.Add(transaction1);
        //            }
        //        }

        //    }

        //}
        //catch (Exception ex)
        //{

        //    throw;
        //}
    }
}
