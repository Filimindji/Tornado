using System;
using System.Windows;
using System.Windows.Input;
using Tornado.ViewModels;

namespace Tornado.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainView
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        protected override async void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            await MainViewModel.LoadPlugins();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                if (DataContext == null)
                    DataContext = new MainViewModel(this);

                return DataContext as MainViewModel;
            }
        }

        private void MainWindow_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        public string StartupPath
        {
            get { return System.AppDomain.CurrentDomain.BaseDirectory; }
        }
    }
}
