using System.Windows;
using Exercise2.ViewModel;

namespace Exercise2
{
    public partial class MainWindow : Window
    {
        public MainWindow(IMainViewModel viewModel)
        {
            InitializeComponent();
        }
    }
}
