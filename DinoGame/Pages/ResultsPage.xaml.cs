using DinoGame.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DinoGame.Pages
{
    public partial class ResultsPage: Page
    {
        public ResultsPage()
        {
            InitializeComponent();

            LoadPlayersData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow rootFrame = (MainWindow)Application.Current.MainWindow;
            rootFrame.MainFrame.Source = new Uri("Pages/MenuPage.xaml", UriKind.Relative);
        }

        private void LoadPlayersData()
        {
            if (App.DbContext.DbStatus)
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

        private void ButtonClearResults_Click(object sender, RoutedEventArgs e)
        {
            App.DbContext.Players.RemoveRange(App.DbContext.Players.ToList());

            App.DbContext.SaveChanges();

            LBoxPlayers.Items.Clear();
        }
    }
}
