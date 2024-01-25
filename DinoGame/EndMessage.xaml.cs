using System.Windows;

namespace DinoGame
{
    public partial class EndMessage: Window
    {
        public EndMessage(double left, double top)
        {
            InitializeComponent();

            Left = left + 80;
            Top = top + 175;
        }

        private void ButtonToMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
