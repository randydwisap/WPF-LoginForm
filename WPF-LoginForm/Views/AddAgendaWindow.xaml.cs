using System.Windows;
using WPF_LoginForm.ViewModels;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for AddAgendaWindow.xaml
    /// </summary>
    public partial class AddAgendaWindow : Window
    {
        public AddAgendaWindow()
        {
            // Membuat instance dari ViewModel
            var viewModel = new AddAgendaWindowViewModel();

            // Menetapkan DataContext ke instance ViewModel
            this.DataContext = viewModel;

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //txtPengirim.Focus();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
