namespace _7777TicketRegistrator.Client
{
    using System;
    using System.IO;
    using System.Windows.Threading;

    using Newtonsoft.Json;

    public partial class App
    {
        public App()
        {
            using (var context = new LotteryDbContext())
            {
                context.Database.EnsureCreated();
            }

            this.DispatcherUnhandledException += this.OnUnhandledException;
            var appDomain = AppDomain.CurrentDomain;
            appDomain.UnhandledException += this.OnAppDomainUnhandledException;
        }

        private void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var exception = (Exception)e.ExceptionObject;
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.AppendAllText(Path.Combine(dir, "lottery-bot-log-appDomain.txt"), JsonConvert.SerializeObject(exception, Formatting.Indented));
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            File.AppendAllText(Path.Combine(dir, "lottery-bot-log-Dispatcher.txt"), JsonConvert.SerializeObject(e.Exception, Formatting.Indented));
            e.Handled = true;
        }
    }
}
