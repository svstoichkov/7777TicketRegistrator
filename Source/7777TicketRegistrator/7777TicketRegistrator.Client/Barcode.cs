namespace _7777TicketRegistrator.Client
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Annotations;

    public class Barcode : INotifyPropertyChanged
    {
        private string status;

        public int Id { get; set; }

        public string Value { get; set; }

        public string Status
        {
            get => this.status;
            set
            {
                this.status = value;
                this.OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
