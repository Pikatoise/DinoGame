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

            if (!App.DbContext.DbStatus)
                ButtonResults.IsEnabled = false;
        }

        private void ButtonPlay_Click(object sender, RoutedEventArgs e)
        {
            MainWindow rootFrame = (MainWindow)Application.Current.MainWindow;
            rootFrame.MainFrame.Source = new Uri("Pages/PlayPage.xaml", UriKind.Relative);
        }

        private void ButtonResults_Click(object sender, RoutedEventArgs e)
        {
            MainWindow rootFrame = (MainWindow)Application.Current.MainWindow;
            rootFrame.MainFrame.Source = new Uri("Pages/ResultsPage.xaml", UriKind.Relative);
        }
    }
}
