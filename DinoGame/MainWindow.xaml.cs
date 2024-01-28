using DinoGame.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DinoGame
{
    public partial class MainWindow: Window
    {
        public DispatcherTimer timerFloor = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 10) };
        public DispatcherTimer timerDinoAnimation = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 150) };
        public DispatcherTimer timerDinoJump = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 10) };
        public DispatcherTimer timerSpeedIncrease = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };
        public DispatcherTimer timerScore = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 125) };

        private const double GAMESPEEDINCREASE = 0.2;
        private readonly Key[] CONTROLKEYS = new Key[] { Key.W, Key.Space, Key.Up };

        private Random rnd = new Random();
        private double gameSpeed = 3.0;
        private int score = 0;
        private bool isGameStarted = false;
        private bool isJumping = false;

        public MainWindow()
        {
            InitializeComponent();

            ImageFloor.Source = BitmapToImage(Properties.Resources.floor);
            ImageBackground.Source = BitmapToImage(Properties.Resources.background);
            ImageDinoLeft.Source = BitmapToImage(Properties.Resources.dino_left);
            ImageDinoRight.Source = BitmapToImage(Properties.Resources.dino_right);
            ImageCactus.Source = BitmapToImage(Properties.Resources.cactus);

            timerFloor.Tick += FloorMoving;
            timerDinoJump.Tick += DinoJump;
            timerDinoAnimation.Tick += DinoAnimation;
            timerSpeedIncrease.Tick += (object? sender, EventArgs e) =>
            {
                new Task(() => gameSpeed += GAMESPEEDINCREASE).Start();
            };
            timerScore.Tick += (object? sender, EventArgs e) =>
            {
                new Task(() => score += (int)gameSpeed).Start();
                TBlockScore.Text = score.ToString();
            };

            //CurrentDino = ImageDinoLeft.Visibility == Visibility.Visible ? ImageDinoLeft : ImageDinoRight;
        }

        private BitmapSource BitmapToImage(System.Drawing.Bitmap bitmap) => Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        private Image CurrentDino => ImageDinoLeft.Visibility == Visibility.Visible ? ImageDinoLeft : ImageDinoRight;

        private void Window_PreviewKeyDown(object? sender, KeyEventArgs e)
        {
            if (isGameStarted && !timerDinoJump.IsEnabled)
                if (CONTROLKEYS.Contains(e.Key))
                {
                    isJumping = true;
                    //currentDino = ImageDinoLeft.Visibility == Visibility.Visible ? ImageDinoLeft : ImageDinoRight;

                    timerDinoAnimation.Stop();
                    timerDinoJump.Start();
                }
        }

        private void DinoAnimation(object? sender, EventArgs e)
        {
            if (ImageDinoLeft.Visibility == Visibility.Visible)
            {
                ImageDinoRight.Visibility = Visibility.Visible;
                ImageDinoLeft.Visibility = Visibility.Hidden;
            }
            else
            {
                ImageDinoLeft.Visibility = Visibility.Visible;
                ImageDinoRight.Visibility = Visibility.Hidden;
            }
        }

        private void FloorMoving(object? sender, EventArgs e)
        {
            if (Canvas.GetLeft(ImageFloor) <= -400)
            {
                Canvas.SetLeft(ImageFloor, 0);
                return;
            }

            if (Canvas.GetRight(ImageCactus) >= 450)
            {
                Canvas.SetRight(ImageCactus, rnd.NextDouble() * (-350 - -150) + -150);
                return;
            }

            Canvas.SetLeft(ImageFloor, Canvas.GetLeft(ImageFloor) - gameSpeed);
            Canvas.SetRight(ImageCactus, Canvas.GetRight(ImageCactus) + gameSpeed);

            // Пересечение кактуса с динозавриком

            //Image cDino = ImageDinoLeft.Visibility == Visibility.Visible ? ImageDinoLeft : ImageDinoRight;

            double xCactus = 370 - Canvas.GetRight(ImageCactus);
            double yCactus = Canvas.GetBottom(ImageCactus);

            /*double xDino = Canvas.GetLeft(cDino);
            double yDino = Canvas.GetBottom(cDino);*/

            double xDino = Canvas.GetLeft(CurrentDino);
            double yDino = Canvas.GetBottom(CurrentDino);


            //if (xDino < xCactus && xDino + cDino.Width >= xCactus && yDino + 5 <= yCactus + ImageCactus.Height)
            if (xDino < xCactus && xDino + CurrentDino.Width >= xCactus && yDino + 5 <= yCactus + ImageCactus.Height)
                StopGame();
        }

        private void DinoJump(object? sender, EventArgs e)
        {
            /*if (isJumping && Canvas.GetBottom(currentDino) < 170)
                Canvas.SetBottom(currentDino, Canvas.GetBottom(currentDino) + gameSpeed * 1.5);
            else
                isJumping = false;

            if (!isJumping && Canvas.GetBottom(currentDino) >= 30)
                Canvas.SetBottom(currentDino, Canvas.GetBottom(currentDino) - gameSpeed * 1.15);

            if (!isJumping && Canvas.GetBottom(currentDino) < 30)
            {
                Canvas.SetBottom(currentDino, 30);

                timerDinoAnimation.Start();
                timerDinoJump.Stop();
            }*/

            if (isJumping && Canvas.GetBottom(CurrentDino) < 170)
                Canvas.SetBottom(CurrentDino, Canvas.GetBottom(CurrentDino) + gameSpeed * 1.5);
            else
                isJumping = false;

            if (!isJumping && Canvas.GetBottom(CurrentDino) >= 30)
                Canvas.SetBottom(CurrentDino, Canvas.GetBottom(CurrentDino) - gameSpeed * 1.15);

            if (!isJumping && Canvas.GetBottom(CurrentDino) < 30)
            {
                Canvas.SetBottom(CurrentDino, 30);

                timerDinoAnimation.Start();
                timerDinoJump.Stop();
            }
        }

        public void StartGame()
        {
            isGameStarted = true;
            ImageDinoLeft.Visibility = Visibility.Visible;

            timerFloor.Start();
            timerDinoAnimation.Start();
            timerSpeedIncrease.Start();
            timerScore.Start();
        }

        public void StopGame()
        {
            isGameStarted = false;
            timerFloor.Stop();
            timerDinoJump.Stop();
            timerScore.Stop();
            timerSpeedIncrease.Stop();
            timerDinoAnimation.Stop();

            EndMessage endMessage = new EndMessage(this.Left, this.Top);
            endMessage.ShowDialog();

            MainFrame.Source = new Uri("Pages/MenuPage.xaml", UriKind.Relative);
            MainFrame.Visibility = Visibility.Visible;

            // Сохранение в бд

            Player? player = App.DbContext.Players.SingleOrDefault(x => x.Nickname.Equals(App.Nickname));

            if (player != null && player.Score < score)
                player.Score = score;
            else if (player == null)
            {
                player = new Player() { Nickname = App.Nickname, Score = score };
                App.DbContext.Players.Add(player);
            }

            App.DbContext.SaveChanges();

            ClearGame();
        }

        private void ClearGame()
        {
            Canvas.SetBottom(ImageFloor, 10);
            Canvas.SetLeft(ImageFloor, 0);

            Canvas.SetBottom(ImageDinoLeft, 30);
            Canvas.SetLeft(ImageDinoLeft, 10);
            ImageDinoLeft.Visibility = Visibility.Hidden;

            Canvas.SetBottom(ImageDinoRight, 30);
            Canvas.SetLeft(ImageDinoRight, 10);
            ImageDinoRight.Visibility = Visibility.Hidden;

            Canvas.SetBottom(ImageCactus, 25);
            Canvas.SetRight(ImageCactus, -150);

            isJumping = false;
            score = 0;
            gameSpeed = 3.0;
            TBlockScore.Text = "";
        }
    }
}
