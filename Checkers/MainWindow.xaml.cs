using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO.Packaging;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Checkers
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        bool moveLeft, moveRight;
        List<Rectangle> itemRemover = new List<Rectangle>();

        Random rand = new Random();

        int enemyImageCounter = 0;
        int enemyImageSpawner = 0;
        int enemyCounter = 100;
        int playerSpeed = 10;
        int limit = 50;
        int score = 0;
        int damage = 0;
        int enemySpeed = 20;

        Rect playerHitBox;

        public MainWindow()
        {
            InitializeComponent();

            string path = Environment.CurrentDirectory + "/Images/Donut.png";
            Canvas.Background = new ImageBrush(new BitmapImage(new Uri(path)));

            Canvas.Focus();

            gameTimer.Interval = TimeSpan.FromMilliseconds(30);
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            ImageBrush playerImage = new ImageBrush();
            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/RAcer.png"));
            player.Fill = playerImage;
        }

        private void MakeEnemies() 
        {
            ImageBrush firstenemyImage = new ImageBrush();

            enemyImageCounter = rand.Next(1, 3);

            switch (enemyImageCounter)
            {
                case 1:
                    firstenemyImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/qq.png"));
                    break;
                case 2:
                    firstenemyImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/Bus.png"));
                    break;
            }

            Rectangle newEnemy = new Rectangle
            {
                Tag = "enemy",
                Height = 84,
                Width = 48,
                Fill = firstenemyImage
            };
            enemyImageSpawner = rand.Next(1, 4);

            switch (enemyImageSpawner) 
            {
                case 1:
                    Canvas.SetTop(newEnemy, -100);
                    Canvas.SetLeft(newEnemy, 230);
                    Canvas.Children.Add(newEnemy);
                    break;
                case 2:
                    Canvas.SetTop(newEnemy, -100);
                    Canvas.SetLeft(newEnemy, 330);
                    Canvas.Children.Add(newEnemy);
                    break;
                case 3:
                    Canvas.SetTop(newEnemy, -100);
                    Canvas.SetLeft(newEnemy, 430);
                    Canvas.Children.Add(newEnemy);
                    break;
            }
        }
        private void GameLoop(object sender, EventArgs e) //метод отвечающий за движение игрока и за спавн противников при условии 
        {
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);
            score += 5;
            scoreText.Content = "Score: " + score;
            enemyCounter -= 1;

            if (enemyCounter < 7)
            {
                MakeEnemies();
                enemyCounter = limit;
            }

            if (moveLeft == true && Canvas.GetLeft(player) > 200) // максимально разрешонное движение игрока влево
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 280 < Application.Current.MainWindow.Width) // максимально разрешонное движение игрока вправо
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            
            foreach (var en in Canvas.Children.OfType<Rectangle>())
            {
                if (en is Rectangle && (string)en.Tag == "enemy")
                {
                    Canvas.SetTop(en, Canvas.GetTop(en) + enemySpeed);

                    if (Canvas.GetTop(en) > 750)
                    {
                        itemRemover.Add(en);
                    }

                    Rect enemyHitBox = new Rect(Canvas.GetLeft(en), Canvas.GetTop(en), en.Width, en.Height); // пересечение с противниками

                    if (playerHitBox.IntersectsWith(enemyHitBox)) // пересечение с противниками
                    {
                        itemRemover.Add(en);
                        damage += 5;
                    }
                }
            }
            
            foreach (Rectangle ene in itemRemover)  
            {
                Canvas.Children.Remove(ene);
            }

            if (score > 660)
            {
                limit = 20;
                enemySpeed = 15;
            }

            if (damage > 4)
            {
                gameTimer.Stop();
                MessageBox.Show("Score " + score + " " + Environment.NewLine + "Game over");

                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }

        private void DKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void UKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
        }
    }
}
