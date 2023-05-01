using ExpenseTrackingAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingAPI.DataContext
{
    public class ExpenseContext : DbContext
    {
        public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options) { }

        public DbSet<Transactions> Transactions { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions>().ToTable("Transaction")
                .HasKey("transaction_id");
            modelBuilder.Entity<TransactionCategory>().ToTable("Transaction_Category")
                .HasKey("category_id");
            modelBuilder.Entity<TransactionType>().ToTable("Transaction_Type")
                .HasKey("transaction_id");
        }
    }
}
