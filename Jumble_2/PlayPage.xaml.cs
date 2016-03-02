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
using System.Runtime.Serialization.Formatters.Binary;

namespace Jumble_2
{
    /// <summary>
    /// Interaction logic for PlayPage.xaml
    /// </summary>
    public partial class PlayPage : Page
    {
        /// <summary>
        /// Private fields
        /// </summary>
        Button[,] frontBoard;
        Dictionary dic;
        Board board;
        int shuffle;
        Stack<Button> buttonStack;
        Brush defaultBrush;
        StringBuilder stringB;
        Button playButton;
        

        /// <summary>
        /// Constructor
        /// </summary>
        public PlayPage(List<string> words, Button playButton)
        {
            InitializeComponent();

            this.playButton = playButton;
            defaultBrush = b11.BorderBrush ;
            shuffle = 3;
            stringB = new StringBuilder();

            //load dictionary
            dic = new Dictionary(words);
            buttonStack = new Stack<Button>();

            //set time
            GlobalVars.time = new TimeSpan(0, 2, 0);
            timeLabel.Content = GlobalVars.time.ToString();

            GlobalVars.successfulWords = new Dictionary<string, int>();

            //set the board
            frontBoard = FrontEndBoard();

            //board maniplator
            board = new Board();
            board.Print(frontBoard);
        }
        public void StartTimer()
        {
            //start timer
            new Thread(Timer).Start();
        }
        /// <summary>
        /// Timer. Runs on a separate thread
        /// </summary>
        private void Timer()
        {
                //Keep running until time reaches 00:00:00
                while (GlobalVars.time.CompareTo(new TimeSpan(0, 0, 0)) > 0)
                {
                    //sleep 1 second
                    Thread.Sleep(1000);

                    //reduce time by 1 second
                    GlobalVars.time = GlobalVars.time.Subtract(new TimeSpan(0, 0, 1));


                    //update time label on the UI
                    Action action = (() =>
                    {
                        if (GlobalVars.time.Seconds <= 30 && GlobalVars.time.Minutes == 0 && timeLabel.Foreground != Brushes.Red)
                            timeLabel.Foreground = Brushes.Red;
                        if (GlobalVars.time.Seconds < 0)
                            GlobalVars.time = new TimeSpan(0, 0, 0);

                        timeLabel.Content = GlobalVars.time.ToString();
                    });
                    Dispatcher.BeginInvoke(action);
                }
                //Invoked when time reaches 0
                //Disables all input fields
                //Game ends
                Action act = () =>
                {
                    shuffleButton.IsEnabled = false;
                    submitButton.IsEnabled = false;
                    playButton.IsEnabled = true;
                    playButton.Content = "New Game";

                    GlobalVars.score = Convert.ToInt32(scoreLabel.Content);
                };
                Dispatcher.BeginInvoke(act);
            }
        

        //Set the lables on the UI as a board
        private Button[,] FrontEndBoard()
        {
            Button[,] board = new Button[6, 6];
            board[0, 0] = b11;
            board[0, 1] = b12;
            board[0, 2] = b13;
            board[0, 3] = b14;
            board[0, 4] = b15;
            board[0, 5] = b16;
            board[1, 0] = b21;
            board[1, 1] = b22;
            board[1, 2] = b23;
            board[1, 3] = b24;
            board[1, 4] = b25;
            board[1, 5] = b26;
            board[2, 0] = b31;
            board[2, 1] = b32;
            board[2, 2] = b33;
            board[2, 3] = b34;
            board[2, 4] = b35;
            board[2, 5] = b36;
            board[3, 0] = b41;
            board[3, 1] = b42;
            board[3, 2] = b43;
            board[3, 3] = b44;
            board[3, 4] = b45;
            board[3, 5] = b46;
            board[4, 0] = b51;
            board[4, 1] = b52;
            board[4, 2] = b53;
            board[4, 3] = b54;
            board[4, 4] = b55;
            board[4, 5] = b56;
            board[5, 0] = b61;
            board[5, 1] = b62;
            board[5, 2] = b63;
            board[5, 3] = b64;
            board[5, 4] = b65;
            board[5, 5] = b66;


            for (int i = 0; i < board.GetLength(0); i++)
                for (int j = 0; j < board.GetLength(1); j++)
                    board[i, j].Click += CellClicked;

            return board;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CellClicked(object sender, RoutedEventArgs e)
        {
            

            Button b = sender as Button;
            
            if(b.BorderBrush == Brushes.Yellow)
            {
                //check if it's last element clicked
                if(b  == buttonStack.Peek())
                {
                    buttonStack.Pop();
                    b.BorderBrush = defaultBrush;
                    stringB.Remove(stringB.Length - 1, 1);
                }
            }
            else
            {
                buttonStack.Push(b);
                b.BorderBrush = Brushes.Yellow;
                stringB.Append(Convert.ToString(b.Content));
            }

            wordLabel.Content = stringB.ToString();
        }
        /// <summary>
        /// Determins the score for a given string input
        /// The longer your string, the higher your multiply score factor
        /// </summary>
        /// <param name="str">The string input</param>
        /// <returns>An integer equilvalent to the score</returns>
        int Score(string str)
        {
            int length = str.Length;
            if (length <= 2)
                return length * 2;
            else if (length <= 4)
                return length * 3;
            else if (length <= 6)
                return length * 4;
            else if (length <= 7)
                return length * 5;
            else if (length <= 8)
                return length * 6;
            else if (length <= 9)
                return length * 7;
            else
                return length * 10;
        }


        /// <summary>
        /// Invoked when submit button is clicked
        /// same things occurs was when enter is pressed
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event arguments</param>
        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            Submit(stringB,buttonStack);

            List<Button> buttons = buttonStack.ToList();
            for (int i = 0; i < buttons.Count; i++)
                buttons[i].BorderBrush = defaultBrush;

            board.ClearString(buttonStack);

            buttonStack.Clear();
            wordLabel.Content = "";
            stringB.Clear();
        }

        /// <summary>
        /// Sumbit a string for validation and checking
        /// </summary>
        /// <param name="word">The input string</param>
        public void Submit(StringBuilder stringB, Stack<Button> buttons)
        {
            //if string is empty
            if (stringB.Length == 0)
                return;

            string str = stringB.ToString();

                //if string is found in dictionary
                if (dic.Find(str))
                {
                    //ger score
                    int s = Score(str);
                    scoreLabel.Content = Convert.ToString(Convert.ToInt32(scoreLabel.Content) + s);

                    //clear the board elements of the string's characters
                    board.ClearString(buttons);
                    //print the new board
                    board.Print(frontBoard);
                //add this string to the "successfull" string list view

                //add 3 seconds of time for every success
                GlobalVars.time = GlobalVars.time.Add(new TimeSpan(0, 0, 3)); //Not thread safe

                if (!GlobalVars.successfulWords.ContainsKey(str))
                    GlobalVars.successfulWords.Add(str, s);
                }
                //Word not found in dictionary
                //subrsact only 2 points
                else
                {
                    scoreLabel.Content = Convert.ToString(Convert.ToInt32(scoreLabel.Content) - 2);
                }
            
        }

        /// <summary>
        /// Invoked when the shuffle button is clicked
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event args</param>
        private void shuffleButton_Click(object sender, RoutedEventArgs e)
        {
            //setup board
            board.Setup();

            //print board 
            board.Print(frontBoard);

            //reduce the number of shuffles left
            shuffle--;
            shuffleLabel.Content = shuffle;

            //disable shuffle is shuffles left is 0
            if (shuffle == 0)
                shuffleButton.IsEnabled = false;
        }
    }


    /// <summary>
    /// The dictioanry of words
    /// </summary>
    public class Dictionary
    {
        private List<string> words;
        public Dictionary(List<string> words)
        {
            this.words = words;
        }
        //find a string in the dictionary
        //Binary search comes in handy since the dictionary is sorted
        public bool Find(string str)
        {
            
            return words.BinarySearch(str.ToLower()) >= 0;
        }
    }

    /// <summary>
    /// The board's behind the scenes class
    /// </summary>
    public class Board
    {
        private const int dimension = 6;
        private char[,] matrix;
        private char[] letters;

        public Board()
        {
            matrix = new char[dimension, dimension];

            //Get the letters
            letters = Enumerable.Range('A', 26).Select(x => (char)x).ToArray();

            //setup the board
            Setup();
        }

        /// <summary>
        /// Randomize the board and letter positions
        /// </summary>
        public void Setup()
        {
            Random rand = new Random();
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    matrix[i, j] = letters[rand.Next(26)];
                }
            }
        }

        /// <summary>
        /// Clear the board on a string pass
        /// </summary>
        /// <param name="str">the string to clear</param>
        public void ClearString(Stack<Button> buttons)
        {
            Random rand = new Random();
            var x = buttons.ToList();
            for (int i = 0; i < buttons.Count; i++)
            {
                x[i].Content = letters[rand.Next(26)];
            }
        }

        /// <summary>
        /// Is the string found on the board?
        /// </summary>
        /// <param name="str">The string in question</param>
        /// <returns>boolean</returns>
        public bool FindString(string str)
        {
            List<char> board = new List<char>();
            for (int i = 0; i < dimension; i++)
                for (int j = 0; j < dimension; j++)
                    board.Add(matrix[i, j]);
            foreach (var val in str)
                if (!board.Contains(val))
                    return false;
            return true;
        }

        /// <summary>
        /// Prints the matrix of characters to the front end
        /// </summary>
        /// <param name="b"></param>
        public void Print(Button[,] b)
        {
            for (int i = 0; i < dimension; i++)
            {
                for (int j = 0; j < dimension; j++)
                {
                    b[i, j].Content = matrix[i, j];
                    b[i, j].FontSize = 18;
                }
            }
        }
    }
}
