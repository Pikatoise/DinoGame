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
    public partial class PlayPage : Page
    {
        public PlayPage()
        {
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow rootFrame = Application.Current.MainWindow as MainWindow;
            rootFrame.MainFrame.Source = new Uri("Pages/MenuPage.xaml", UriKind.Relative);
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TBoxNickname.Text))
            {
                MessageBox.Show("Введите никнейм!","Ошибка",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }

            App.Nickname = TBoxNickname.Text;

            MainWindow rootFrame = Application.Current.MainWindow as MainWindow;
            rootFrame.MainFrame.Visibility = Visibility.Collapsed;
            rootFrame.MainFrame.Source = new Uri("Pages/MenuPage.xaml", UriKind.Relative);

            rootFrame.StartGame();
        }
    }
}
