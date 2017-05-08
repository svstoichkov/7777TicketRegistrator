namespace _7777TicketRegistrator.Client
{
    using System.Text;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    public class BarcodeScannerHandler : Behavior<Window>
    {
        private readonly StringBuilder barcodeReader;
        private readonly KeyConverter keyConverter;
        private bool isScanning;

        public BarcodeScannerHandler()
        {
            this.barcodeReader = new StringBuilder();
            this.keyConverter = new KeyConverter();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewKeyDown += this.OnPreviewKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewKeyDown -= this.OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.F11 || e.Key == Key.F20) && !this.isScanning)
            {
                    this.isScanning = true;
                    e.Handled = true;
            }
            else if ((e.Key == Key.F12 || e.Key == Key.F22) && this.isScanning)
            {
                if (this.barcodeReader.Length != 0)
                {
                    ((MainWindow)this.AssociatedObject).HandleBarcode(this.barcodeReader.ToString().Trim('L'));
                    this.barcodeReader.Clear();
                }

                this.isScanning = false;
                e.Handled = true;
            }
            else if (this.isScanning)
            {
                e.Handled = true;
                var xChar = this.keyConverter.ConvertToString(e.Key);
                this.barcodeReader.Append(xChar);
            }
        }
    }
}