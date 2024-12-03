using System.Windows;
using System.Windows.Controls;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class SuratMasukView : UserControl
    {
        public SuratMasukView()
        {
            InitializeComponent();
        }

        private void txtsearch_GotFocus(object sender, RoutedEventArgs e)
        {
            var txtsearch = sender as TextBox;
            if (txtsearch != null)
            {
                txtsearch.Text = string.Empty;
            }
        }
    }
}
