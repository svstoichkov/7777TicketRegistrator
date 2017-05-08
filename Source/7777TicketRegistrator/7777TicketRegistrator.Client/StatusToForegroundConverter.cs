namespace _7777TicketRegistrator.Client
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    public class StatusToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = value as string;
            if (status == null)
            {
                return Brushes.Black;
            }

            status = status.ToLower();

            if (status.Contains("печели"))
            {
                return Brushes.Green;
            }

            if (status.Contains(""))
            {

            }

            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
