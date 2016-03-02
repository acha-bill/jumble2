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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Jumble_2
{
    /// <summary>
    /// Interaction logic for GameOverPage.xaml
    /// </summary>
    public partial class GameOverPage : Page
    {
        private Brush defaultStarBrush;
        private Brush yellowBrush;
        public GameOverPage()
        {
            InitializeComponent();

            defaultStarBrush = star1.Fill;
            yellowBrush = Brushes.Gold;

            foreach(var val in GlobalVars.successfulWords)
                listView.Items.Add(new ListViewItem { Word = val.Key, Score = val.Value });

            scoreLabel.Content = GlobalVars.score;
            if(GlobalVars.score < 50)
            { }
            else if(GlobalVars.score < 100)
            {
                star1.Fill = yellowBrush;
            }
            else if(GlobalVars.score < 200)
            {
                star1.Fill = yellowBrush;
                star2.Fill = yellowBrush;
            }
            else
            {
                star1.Fill = yellowBrush;
                star2.Fill = yellowBrush;
                star3.Fill = yellowBrush;
            }

            SaveScore();

        }
        public void SaveScore()
        {
            List<int> scores = new List<int>();
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fstream = new FileStream("scores.dat", FileMode.OpenOrCreate))
            {
                
                if(fstream.Length > 0)
                {
                    scores = formatter.Deserialize(fstream) as List<int>;
                }
                scores.Add(GlobalVars.score);
            }
            using (FileStream fstream = new FileStream("scores.dat", FileMode.Create))
            {
                formatter.Serialize(fstream, scores);
            }
        }
        /// <summary>
        /// Helper class for listview 
        /// </summary>
        class ListViewItem
        {
            public string Word { get; set; }
            public int Score { get; set; }
        }
    }
}
