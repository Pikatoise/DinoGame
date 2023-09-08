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
using System.Windows.Shapes;

namespace DinoGame
{
    public partial class EndMessage : Window
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
