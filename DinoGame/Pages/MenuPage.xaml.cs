using DinoGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DinoGame.Pages
{
    public partial class MenuPage : Page
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
