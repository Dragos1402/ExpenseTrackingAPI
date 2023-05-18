using ExpenseTrackingAPI.Interfaces;
using Microsoft.Data.SqlClient;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Transactions;
using ExpenseTrackingAPI.Models.DbModels;
using System.Data;
using ExpenseTrackingAPI.DataContext;
using ExpenseTrackingAPI.Models.TransactionModels;
using ExpenseTrackingAPI.Helpers;

namespace ExpenseTrackingAPI.Services
{
    public class TransactionsServ : ITransactions
    {
        public List<Models.TransactionModels.TransactionList> Transactions { get; set; }
        public Models.TransactionModels.Transaction Transaction { get; set; }
        public object ResultID { get; set; }

        private readonly ExpenseContext _context;

        public TransactionsServ(IConfiguration config)
        {
            _context = new(config);
        }
        public string AddTransaction(AddTransaction addTransaction)
        {
            try
            {
                var category = _context.TransactionCategories.FirstOrDefault(c => c.category_id == addTransaction.category_id);
                var type = _context.TransactionTypes.FirstOrDefault(t => t.type_id == addTransaction.type_id);

                var newTransaction = new Models.DbModels.Transactions
                {
                    value = addTransaction.value,
                    transaction_date = addTransaction.transaction_date,
                    note = addTransaction.note,
                    category = category,
                    type = type,
                    inserted_by = 2,
                    inserted_at_date = DateTime.Now,
                };
                _context.Transactions.Add(newTransaction);
                _context.SaveChanges();

                this.ResultID = newTransaction.transaction_id;
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                return ErrorCodes.DATABASE_WRITING_ERROR;
            }
        }

        public string GetTransactions()
        {
            Transactions = new List<Models.TransactionModels.TransactionList>();
            try
            {
                Transactions = (from d in _context.Transactions
                                      select new Models.TransactionModels.TransactionList
                                      {
                                          transaction_id = d.transaction_id,
                                          value = d.value,
                                          transaction_date = d.transaction_date,
                                          note = d.note,
                                          category = d.category,
                                          type = d.type
                                      }).ToList();
                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
                return ErrorCodes.DATABASE_READING_ERROR;
            }
        }

        public string GetTransactionById(int id)
        {
            this.Transaction = new Models.TransactionModels.Transaction();
            try
            {
                if (id==null)
                {
                    return ErrorCodes.NO_ID_PROVIDED;
                }
                else
                {
                    Transaction = (from d in _context.Transactions
                                   where d.transaction_id == id
                                   select new Models.TransactionModels.Transaction
                                   {
                                       transaction_id = d.transaction_id,
                                       value=d.value,
                                       transaction_date = d.transaction_date,
                                       note = d.note,
                                       
                                   }).FirstOrDefault();                
                }
                return ErrorCodes.SUCCESS;
            }
            catch (Exception)
            {
                return ErrorCodes.DATABASE_READING_ERROR;
            }
        }

        public string UpdateTransaction(UpdateTransaction updateTransaction)
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
                    transaction.modified_by = 2;
                    transaction.modified_at_date = DateTime.Now;

                    _context.SaveChanges();
                }

                this.ResultID = updateTransaction.transaction_id;

                return ErrorCodes.SUCCESS;
            }
            catch (Exception ex)
            {
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
