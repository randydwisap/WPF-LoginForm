using System;
using System.Globalization;
using System.Windows.Data;

namespace WPF_LoginForm.Converters
{
    public class UriConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                return new Uri(path); // Mengonversi path string menjadi Uri
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Tidak perlu implementasi untuk ConvertBack
            throw new NotImplementedException();
        }
    }
}
