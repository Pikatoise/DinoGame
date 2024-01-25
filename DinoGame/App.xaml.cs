using DinoGame.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows;

namespace DinoGame
{
    public partial class App: Application
    {
        public static GamedbContext DbContext = new GamedbContext(new DbContextOptionsBuilder<GamedbContext>()
            .UseSqlite("Filename=gamedb")
            .Options);

        public static string Nickname = "Гость";
    }
}
