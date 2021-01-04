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
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;

namespace WPF_Balloon_Popping_Game_Moo_ICT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();

        int speed = 3;
        int intervals = 90;
        Random rand = new Random();

        List<Rectangle> itemRemover = new List<Rectangle>();

        ImageBrush backgroundImage = new ImageBrush();

        int balloonSkins;
        int i;

        int missedBalloons;

        bool gameIsActive;

        int score;

        MediaPlayer player = new MediaPlayer();


        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameEngine;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            backgroundImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/files/background-Image.jpg"));
            MyCanvas.Background = backgroundImage;

            RestartGame();
        }

        private void GameEngine(object sender, EventArgs e)
        {
            scoreText.Content = "Score: " + score;

            intervals -= 10;

            if (intervals < 1)
            {
                ImageBrush balloonImage = new ImageBrush();

                balloonSkins += 1;

                if (balloonSkins > 5)
                {
                    balloonSkins = 1;
                }

                switch (balloonSkins)
                {
                    case 1: 
                       balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/files/balloon1.png"));
                        break;
                    case 2:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/files/balloon2.png"));
                        break;
                    case 3:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/files/balloon3.png"));
                        break;
                    case 4:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/files/balloon4.png"));
                        break;
                    case 5:
                        balloonImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/files/balloon5.png"));
                        break;
                }

                Rectangle newBalloon = new Rectangle
                {
                    Tag = "balloon",
                    Height = 70,
                    Width = 50,
                    Fill = balloonImage
                };

                Canvas.SetLeft(newBalloon, rand.Next(50, 400));
                Canvas.SetTop(newBalloon, 600);

                MyCanvas.Children.Add(newBalloon);

                intervals = rand.Next(90, 150);
            }

            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {

                if ((string)x.Tag == "balloon")
                {

                    i = rand.Next(-5, 5);

                    Canvas.SetTop(x, Canvas.GetTop(x) - speed);
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - (i * -1));

                }

                if (Canvas.GetTop(x) < 20)
                {
                    itemRemover.Add(x);

                    missedBalloons += 1;
                }


            }


            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }



            if (missedBalloons > 10)
            {
                gameIsActive = false;
                gameTimer.Stop();
                MessageBox.Show("Game over!! You missed 10 Balloons" + Environment.NewLine + "Click ok to play again");

                RestartGame();
            }

            if (score > 3)
            {
                speed = 7;
            }



        }

        private void PopBalloons(object sender, MouseButtonEventArgs e)
        {
            if (gameIsActive)
            {

                if (e.OriginalSource is Rectangle)
                {

                    Rectangle activeRec = (Rectangle)e.OriginalSource;

                    player.Open(new Uri("../../files/pop_sound.mp3", UriKind.RelativeOrAbsolute));
                    player.Play();

                    MyCanvas.Children.Remove(activeRec);

                    score += 1;
                }
            }
        }

        private void StartGame()
        {
            gameTimer.Start();

            missedBalloons = 0;
            score = 0;
            intervals = 90;
            gameIsActive = true;
            speed = 3;
        }

        private void RestartGame()
        {
            foreach (var x in MyCanvas.Children.OfType<Rectangle>())
            {
                itemRemover.Add(x);
            }

            foreach (Rectangle y in itemRemover)
            {
                MyCanvas.Children.Remove(y);
            }

            itemRemover.Clear();

            StartGame();



        }

    }
}
