namespace _7777TicketRegistrator.Client
{
    using Microsoft.EntityFrameworkCore;

    public class LotteryDbContext : DbContext
    {
        public DbSet<Barcode> Barcodes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=db.db");
        }
    }
}
