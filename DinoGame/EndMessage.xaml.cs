using System.Windows;

namespace DinoGame
{
    public partial class EndMessage: Window
    {
        public EndMessage(double left, double top)
        {
            InitializeComponent();

            this.Left = left + 80;
            this.Top = top + 175;
        }

        private void ButtonToMenu_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
