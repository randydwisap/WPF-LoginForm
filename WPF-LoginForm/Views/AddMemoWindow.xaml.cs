using System.Windows;
using WPF_LoginForm.ViewModels;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for AddAgendaWindow.xaml
    /// </summary>
    public partial class AddMemoWindow : Window
    {
        public AddMemoWindow()
        {
            // Membuat instance dari ViewModel
            var viewModel = new AddMemoWindowViewModel();

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
