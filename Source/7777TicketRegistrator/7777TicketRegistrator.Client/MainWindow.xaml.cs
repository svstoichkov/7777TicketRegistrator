namespace _7777TicketRegistrator.Client
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;

    using Microsoft.EntityFrameworkCore;

    using Properties;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            using (var context = new LotteryDbContext())
            {
                this.Barcodes = new ObservableCollection<Barcode>(context.Barcodes);
            }

            this.DataContext = this;
            this.tbUsername.Text = Settings.Default.Username;
            this.tbPassword.Text = Settings.Default.Password;
        }

        public ObservableCollection<Barcode> Barcodes { get; }

        public void HandleBarcode(string value)
        {
            if (this.Barcodes.All(x => x.Value != value))
            {
                var barcode = new Barcode { Value = value };
                this.Barcodes.Add(barcode);
                this.dgBarcodes.ScrollIntoView(barcode);
                using (var context = new LotteryDbContext())
                {
                    context.Barcodes.Add(barcode);
                    context.SaveChanges();
                }
            }
        }

        private async void BtnStart_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                this.btnStart.IsEnabled = false;
                using (var client = new LotteryClient())
                {
                    var loginResult = await client.Login(this.tbUsername.Text, this.tbPassword.Text);
                    if (loginResult)
                    {
                        Settings.Default.Username = this.tbUsername.Text;
                        Settings.Default.Password = this.tbPassword.Text;
                        Settings.Default.Save();

                        while (true)
                        {
                            foreach (var barcode in this.Barcodes.Where(x => string.IsNullOrWhiteSpace(x.Status)).ToList())
                            {
                                var status = await client.Register(barcode.Value);
                                barcode.Status = status;

                                using (var context = new LotteryDbContext())
                                {
                                    context.Entry(barcode).State = EntityState.Modified;
                                    context.SaveChanges();
                                }
                            }

                            await Task.Delay(500);
                        }
                    }
                    else
                    {
                        MessageBox.Show(this, "Грешно име или парола", "Грешка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            finally
            {
                this.btnStart.IsEnabled = true;
            }
        }
    }
}