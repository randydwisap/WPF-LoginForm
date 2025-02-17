using System.Windows;
using WPF_LoginForm.Repositories;
using WPF_LoginForm.ViewModels;

namespace WPF_LoginForm.Views
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        // Konstruktor baru yang menerima agendaId
        private int _agendaId;
        public PopupWindow(int agendaId)
        {
            InitializeComponent();
            _agendaId = agendaId; // Set the agendaId to be used in this window
            // Membuat ViewModel dengan agendaId yang diteruskan
            var viewModel = new PopupWindowViewModel(new AnggotaAgendaRepository(), agendaId);
            DataContext = viewModel;  // Menetapkan DataContext ke ViewModel
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
