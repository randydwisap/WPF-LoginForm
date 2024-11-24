using System.Windows;
using System.Windows.Controls;
using WPF_LoginForm.ViewModels;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class TambahKaryawan : Window
    {
        public TambahKaryawan()
        {
            // Membuat instance dari ViewModel
            var viewModel = new TambahKaryawanViewModel();

            // Menetapkan DataContext ke instance ViewModel
            this.DataContext = viewModel;

            InitializeComponent();
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUsername.Focus();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var password = (sender as PasswordBox).Password;
            // Simpan password ke ViewModel
           TambahKaryawanViewModel viewModel = this.DataContext as TambahKaryawanViewModel;
            viewModel.Password = password;
        }
    }
}
