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
using System.Threading;
using System.IO;
using System.Media;

namespace Jumble_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread soundThread;
        PlayPage gamePage;
        GameOverPage gameOverpage;
        Thread transitionThread;

        public MainWindow()
        {
            InitializeComponent();
            this.Closed += MainWindow_Closed;

            soundThread = new Thread(GameSound);
            soundThread.Start();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        public void  GameSound()
        {
            SoundPlayer player = new SoundPlayer("sound.wav");
            player.PlayLooping();           
        }
        public List<string> Dic(string filename)
        {
            List<string> words = new List<string>();
            using (TextReader reader = new StreamReader(filename))
            {
                words = reader.ReadToEnd().Split().ToList();
                words.RemoveAll(x => x == "");
            }
            return words;
        }
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            gamePage = new PlayPage(Dic("words.txt"), playButton);
            frame.Navigate(gamePage);
            gamePage.StartTimer();
            playButton.IsEnabled = false;


            transitionThread = new Thread(ShowGameOverPage);
            transitionThread.SetApartmentState(ApartmentState.STA);
            transitionThread.Start();
        }
        private void ShowGameOverPage()
        {
            while(!GlobalVars.time.Equals(new TimeSpan(0,0,0)))
            {
            }
            Action action = (() =>
            {
                    gameOverpage = new GameOverPage();
                    frame.Navigate(gameOverpage);
            });
            Dispatcher.BeginInvoke(action);
        }

        private void rulesButton_Click(object sender, RoutedEventArgs e)
        {
            playButton.IsEnabled = true;
            RulesPage rp = new RulesPage();
            frame.Navigate(rp);
        }

        private void LeaderBoardButton_Click(object sender, RoutedEventArgs e)
        {
            playButton.IsEnabled = true;
            LeaderBoardPage lp = new LeaderBoardPage();
            frame.Navigate(lp);
        }
    }
}
