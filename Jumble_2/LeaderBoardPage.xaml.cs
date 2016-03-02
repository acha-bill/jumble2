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
    /// Interaction logic for LeaderBoardPage.xaml
    /// </summary>
    public partial class LeaderBoardPage : Page
    {
        List<int> scores;
        BinaryFormatter formatter;
        public LeaderBoardPage()
        {
            InitializeComponent();
            scores = new List<int>();
            formatter = new BinaryFormatter();

            LoadLastScores();

        }
        void LoadLastScores()
        {
            using (FileStream fstream = new FileStream("scores.dat", FileMode.OpenOrCreate))
            {
                if (fstream.Length > 0)
                {
                    scores = formatter.Deserialize(fstream) as List<int>;
                }
            }


            if (scores.Count > 0)
            {
                int highscore = scores.Max();
                highScoreLabel.Content = highscore;

                for (int i = 0; i < 5 && i < scores.Count; i++)
                {
                    lastGamesListView.Items.Add((new ListViewItem1 { Score = scores[i] }));
                }
            }

        }
        class ListViewItem1
        {
            public int Score { get; set; }
        }
    }
}
