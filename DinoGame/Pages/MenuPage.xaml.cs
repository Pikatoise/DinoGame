using System;
using System.Windows;
using System.Windows.Controls;

namespace DinoGame.Pages
{
    public partial class MenuPage: Page
    {
        public MenuPage()
        {
            InitializeComponent();

            if (!App.DbStatus)
                ButtonResults.IsEnabled = false;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            MainWindow rootFrame = Application.Current.MainWindow as MainWindow;
            rootFrame.MainFrame.Source = new Uri("Pages/PlayPage.xaml", UriKind.Relative);
        }

        private void ButtonResults_Click(object sender, RoutedEventArgs e)
        {
            MainWindow rootFrame = Application.Current.MainWindow as MainWindow;
            rootFrame.MainFrame.Source = new Uri("Pages/ResultsPage.xaml", UriKind.Relative);
        }
    }
}
