namespace _7777TicketRegistrator.Client
{
    public partial class App
    {
        public App()
        {
            using (var context = new LotteryDbContext())
            {
                context.Database.EnsureCreated();
            }
        }
    }
}
