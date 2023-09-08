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
    public partial class ResultsPage : Page
    {
        public ResultsPage()
        {
            InitializeComponent();

            LoadPlayersData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow rootFrame = Application.Current.MainWindow as MainWindow;
            rootFrame.MainFrame.Source = new Uri("Pages/MenuPage.xaml", UriKind.Relative);
        }

        void LoadPlayersData()
        {
            if (App.DbStatus)
            {
                List<Player> players = App.DbContext.Players.ToList();

                players = players.OrderBy(x => x.Score).Reverse().ToList();

                LBoxPlayers.Items.Clear();

                for (int i = 0; i < players.Count; i++)
                {
                    ListBoxItem lbitem = new ListBoxItem()
                    {
                        Width = 350,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 5, 0, 0),
                        Content = $"{players[i].Nickname} | {players[i].Score}"
                    };

                    lbitem.FontSize = 14;

                    if (i == 0)
                        lbitem.FontSize = 22;
                    else if (i == 1) 
                        lbitem.FontSize = 18;
                    else if (i == 2)
                        lbitem.FontSize = 16;

                    LBoxPlayers.Items.Add(lbitem);
                }
            }
        }
    }
}
