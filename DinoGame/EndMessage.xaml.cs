using System.Windows;

namespace DinoGame
{
    public partial class EndMessage: Window
    {
        public EndMessage()
        {
            InitializeComponent();
        }

        private void ButtonToMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
