using Microsoft.EntityFrameworkCore;

namespace BankTrans.Data
{

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions)
            : base(dbContextOptions)
        {
        }

        public DbSet<CityBankTransaction> CityBankTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
