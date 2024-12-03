using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for MainWindowView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnlControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnSuratMasuk_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            // Cek jika tombol yang ditekan adalah Alt + S
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                MessageBox.Show("Alt + S Terdeteksi");
                // Pilih RadioButton Surat Masuk
                btnSuratMasuk.IsChecked = true;
            }
            // Cek jika tombol yang ditekan adalah Alt + K
            else if (e.Key == Key.K && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                // Pilih RadioButton Surat Keluar
                btnSuratKeluar.IsChecked = true;
            }
            else if (e.Key == Key.D && (Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
            {
                // Pilih RadioButton Surat Keluar
                btnDisposisi.IsChecked = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }
    }
}
