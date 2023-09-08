using DinoGame.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace DinoGame
{
    public partial class App : Application
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
