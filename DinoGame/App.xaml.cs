using DinoGame.Models;
using System.Windows;

namespace DinoGame
{
    public partial class App: Application
    {
        public static GamedbContext DbContext;

        public static bool DbStatus
        {
            get
            {
                return DbContext.Database.CanConnect();
            }
        }

        public static string Nickname;

        public App()
        {
            DbContext = new GamedbContext();
        }
    }
}
