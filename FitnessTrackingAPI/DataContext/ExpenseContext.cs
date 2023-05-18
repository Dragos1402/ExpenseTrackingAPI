using ExpenseTrackingAPI.Models.DbModels;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTrackingAPI.DataContext
{
    public class ExpenseContext : DbContext
    {
        private readonly IConfiguration? config;
        public ExpenseContext()
        {
        }
        public ExpenseContext(IConfiguration config)
        {
            this.config = config;
        }
        public ExpenseContext(DbContextOptions<ExpenseContext> options) : base(options) { }

        public DbSet<Transactions> Transactions { get; set; } = null;
        public DbSet<TransactionCategory> TransactionCategories { get; set; } = null;
        public DbSet<TransactionType> TransactionTypes { get; set; } = null;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transactions>().ToTable("Transaction")
                .HasKey("transaction_id");
            modelBuilder.Entity<TransactionCategory>().ToTable("TransactionCategories")
                .HasKey("category_id");
            modelBuilder.Entity<TransactionType>().ToTable("TransactionTypes")
                .HasKey("type_id");
        }
    }
}
