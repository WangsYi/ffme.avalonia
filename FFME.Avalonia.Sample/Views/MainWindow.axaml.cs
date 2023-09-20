using Avalonia.Controls;
using Avalonia.Interactivity;
using FFME.Avalonia.Sample.ViewModels;

namespace FFME.Avalonia.Sample.Views
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;
        public MainWindow()
        {
            InitializeComponent();
            _vm = new MainWindowViewModel();
            DataContext = _vm;
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object? sender, RoutedEventArgs e)
        {
            _vm.LoadedCommand.Execute(Media);
        }
    }
}