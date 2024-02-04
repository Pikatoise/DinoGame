using DinoGame.Models;
using System;
using System.Linq;
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
        // Объявление переменных, таймеров и свойств 

        public DispatcherTimer timerWorld = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(2.0)
        };
        public DispatcherTimer timerDinoAnimation = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(150.0)
        };
        public DispatcherTimer timerDinoJump = new DispatcherTimer(DispatcherPriority.Render)
        {
            Interval = TimeSpan.FromMilliseconds(2.0)
        };
        public DispatcherTimer timerSpeedIncrease = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(1000.0) };
        public DispatcherTimer timerScore = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(125.0) };

        private const int XDINO = 10;
        private const int YDINO = 30;
        private const int CACTUSMINSTARTPOSITION = -250;
        private const int CACTUSMAXSTARTPOSITION = -600;
        private const int FLOORHEIGHT = 30;
        private const double GAMESPEEDINCREASE = 0.4;
        private const double FLOORCHANGEPOSITION = 600;
        private const double CACTUSRELOADPOSITION = 600;
        private const double SCOREFACTOR = 0.7;
        private const double BASEJUMPFACTOR = 3;
        private const double JUMPFACTORSTEP = 0.1;
        private const double JUMPSPEED = 3.5;

        private readonly Key[] CONTROLKEYS = new Key[] { Key.W, Key.Space, Key.Up };

        private BitmapSource dinoLeftFoot = BitmapToImage(Properties.Resources.dino_left); // Изображеине динозаврика на левой ноге
        private BitmapSource dinoRightFoot = BitmapToImage(Properties.Resources.dino_right); // Изображеине динозаврика на правой ноге
        private BitmapSource dinoDead = BitmapToImage(Properties.Resources.dino_dead);
        private Random rnd = new Random();
        private bool isGameStarted = false;
        private bool isJumping = false; // Летит ли динозаврик
        private bool dinoFoot = false; // Левая или правая нога динозаврика
        private int score = 0;
        private double gameSpeed = 3.0;
        private double dinoJumpFactor = BASEJUMPFACTOR; // Зависимость от времени полета

        // Установка игровых изображений и привязка действий к таймерам
        public MainWindow()
        {
            InitializeComponent();

            ImageFloorFirst.Source = BitmapToImage(Properties.Resources.floor);
            ImageFloorSecond.Source = BitmapToImage(Properties.Resources.floor);
            ImageBackground.Source = BitmapToImage(Properties.Resources.background);
            ImageCactus.Source = BitmapToImage(Properties.Resources.cactus);
            ImageDino.Source = dinoFoot ? dinoLeftFoot : dinoRightFoot;

            Canvas.SetLeft(ImageDino, XDINO);
            Canvas.SetBottom(ImageDino, YDINO);

            timerWorld.Tick += FloorMoving;
            timerWorld.Tick += CactusMoving;
            timerDinoJump.Tick += DinoJump;
            timerDinoAnimation.Tick += DinoAnimation;
            timerSpeedIncrease.Tick += (object? sender, EventArgs e) =>
            {
                gameSpeed += GAMESPEEDINCREASE;
            };
            timerScore.Tick += (object? sender, EventArgs e) =>
            {
                score += (int)(gameSpeed * SCOREFACTOR);
                TBlockScore.Text = score.ToString();
            };
        }

        /// <summary>
        /// Метод для запуска игры
        /// </summary>
        public void StartGame()
        {
            isGameStarted = true;
            ImageDino.Visibility = Visibility.Visible;

            timerWorld.Start();
            timerDinoAnimation.Start();
            timerSpeedIncrease.Start();
            timerScore.Start();
        }

        /// <summary>
        /// Метод для остановки игры
        /// </summary>
        public void StopGame()
        {
            isGameStarted = false;
            timerWorld.Stop();
            timerDinoJump.Stop();
            timerScore.Stop();
            timerSpeedIncrease.Stop();
            timerDinoAnimation.Stop();

            ImageDino.Source = dinoDead;

            EndMessage endMessage = new EndMessage();
            endMessage.ShowDialog();

            MainFrame.Source = new Uri("Pages/MenuPage.xaml", UriKind.Relative);
            MainFrame.Visibility = Visibility.Visible;

            SaveGame();

            ClearGame();
        }

        // Преобразование карты битов в изображение
        private static BitmapSource BitmapToImage(System.Drawing.Bitmap bitmap) =>
            Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

        // Управление динозавриком
        private void Window_PreviewKeyDown(object? sender, KeyEventArgs e)
        {
            if (isGameStarted && !timerDinoJump.IsEnabled)
                if (CONTROLKEYS.Contains(e.Key))
                {
                    isJumping = true;

                    timerDinoAnimation.Stop();
                    timerDinoJump.Start();
                }
        }

        // Анимация бега
        private void DinoAnimation(object? sender, EventArgs e)
        {
            if (dinoFoot)
            {
                ImageDino.Source = dinoRightFoot;
                dinoFoot = !dinoFoot;
            }
            else
            {
                ImageDino.Source = dinoLeftFoot;
                dinoFoot = !dinoFoot;
            }
        }

        // Передвижение пола
        private void FloorMoving(object? sender, EventArgs e)
        {
            double floorFirstLeft = Canvas.GetLeft(ImageFloorFirst);
            double floorSecondLeft = Canvas.GetLeft(ImageFloorSecond);
            double currentGameSpeed = gameSpeed;

            double divider = 5.0;

            for (int i = 0; i < (int)divider; i++)
            {
                floorFirstLeft -= currentGameSpeed / divider;
                floorSecondLeft -= currentGameSpeed / divider;

                Canvas.SetLeft(ImageFloorFirst, floorFirstLeft);
                Canvas.SetLeft(ImageFloorSecond, floorSecondLeft);
            }

            if (floorFirstLeft <= FLOORCHANGEPOSITION * -1)
            {
                Canvas.SetLeft(ImageFloorFirst, FLOORCHANGEPOSITION);
                Canvas.SetLeft(ImageFloorSecond, 0);
                return;
            }

            if (floorSecondLeft <= FLOORCHANGEPOSITION * -1)
            {
                Canvas.SetLeft(ImageFloorSecond, FLOORCHANGEPOSITION);
                Canvas.SetLeft(ImageFloorFirst, 0);
                return;
            }
        }

        // Передвижение кактуса
        private void CactusMoving(object? sender, EventArgs e)
        {
            double cactusRight = Canvas.GetRight(ImageCactus);
            double currentGameSpeed = gameSpeed;

            if (cactusRight >= CACTUSRELOADPOSITION)
            {
                double cactusNewPosFactor = Math.Round(rnd.NextDouble(), 1);
                double cactusNewPos =
                    cactusNewPosFactor * (CACTUSMAXSTARTPOSITION - CACTUSMINSTARTPOSITION) + CACTUSMINSTARTPOSITION;

                Canvas.SetRight(ImageCactus, cactusNewPos);
                return;
            }

            double divider = 5.0;

            for (int i = 0; i < (int)divider; i++)
            {
                cactusRight += currentGameSpeed / divider;
                Canvas.SetRight(ImageCactus, cactusRight);
            }

            if (CollisionCheck())
                StopGame();
        }

        // Проверка столкновения динозаврика с кактусом
        private bool CollisionCheck()
        {
            double xCactus = Width - Canvas.GetRight(ImageCactus) - ImageCactus.Width;
            double yCactus = Canvas.GetBottom(ImageCactus);
            double xDino = Canvas.GetLeft(ImageDino);
            double yDino = Canvas.GetBottom(ImageDino);

            Rect Cactus = new Rect()
            {
                Location = new Point(xCactus, yCactus),
                Size = new Size(ImageCactus.Width, ImageCactus.Height)
            };

            Rect Dino = new Rect()
            {
                Location = new Point(xDino + 10, yDino + 10),
                Size = new Size(ImageDino.Width - 10, ImageDino.Height)
            };

            return Cactus.IntersectsWith(Dino);
        }

        // Анимация прыжка
        private void DinoJump(object? sender, EventArgs e)
        {
            double dinoBottom = Canvas.GetBottom(ImageDino);

            if (isJumping)
            {
                if (dinoJumpFactor < 0.1)
                {
                    isJumping = false;
                    dinoJumpFactor = 0.1;
                }
                else
                {
                    Canvas.SetBottom(ImageDino, dinoBottom + JUMPSPEED * dinoJumpFactor);

                    dinoJumpFactor -= JUMPFACTORSTEP;

                    return;
                }
            }

            if (!isJumping && dinoBottom > FLOORHEIGHT && dinoJumpFactor >= 0.1)
            {
                Canvas.SetBottom(ImageDino, dinoBottom - JUMPSPEED * dinoJumpFactor);

                dinoJumpFactor += JUMPFACTORSTEP;

                return;
            }

            Canvas.SetBottom(ImageDino, FLOORHEIGHT);

            dinoJumpFactor = BASEJUMPFACTOR;

            timerDinoAnimation.Start();
            timerDinoJump.Stop();
        }

        // Сохранение результата
        private void SaveGame()
        {
            Player? player = App.DbContext.Players.SingleOrDefault(x => x.Nickname.Equals(App.Nickname));

            if (player != null && player.Score < score)
                player.Score = score;
            else if (player == null)
            {
                player = new Player() { Nickname = App.Nickname, Score = score };
                App.DbContext.Players.Add(player);
            }

            App.DbContext.SaveChanges();
        }

        // Очистка игры
        private void ClearGame()
        {
            Canvas.SetLeft(ImageFloorFirst, 0);
            Canvas.SetLeft(ImageFloorSecond, 600);

            Canvas.SetBottom(ImageDino, 30);
            Canvas.SetLeft(ImageDino, 10);
            ImageDino.Visibility = Visibility.Hidden;

            Canvas.SetRight(ImageCactus, -150);

            isJumping = false;
            score = 0;
            gameSpeed = 3.0;
            TBlockScore.Text = "";
        }
    }
}
