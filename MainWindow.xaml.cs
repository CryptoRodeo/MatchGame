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

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        private List<List<string>> _gameList;

        private List<List<string>> emojiList
       {
            get 
            {
                if (_gameList.Count == 0)
                {
                    _gameList.Add(new List<string> { "🐵", "🐵", "🐶", "🐶", "🐺", "🐺", "🐱", "🐱", "🦁", "🦁", "🐯", "🐯", "🦒", "🦒", "🦊", "🦊" });
                    _gameList.Add(new List<string> { "🍇","🍇","🍉","🍉","🥭","🥭","🍎","🍎","🍑","🍑", "🍍", "🍍", "🥕","🥕","🥑","🥑"});
                    _gameList.Add(new List<string> { "💯", "💢", "💈", "💘", "♠", "📢", "♂", "♀", "💯", "💢", "💈", "💘", "♠", "📢", "♂", "♀" });
                    _gameList.Add(new List<string> { "😀", "😅", "😍", "🤑", "😏", "🤢", "😳", "🤡", "😀", "😅", "😍", "🤑", "😏", "🤢", "😳", "🤡" });
                }
                return _gameList;
            }
       }

        
        public MainWindow()
        {
            _gameList = new List<List<string>>();
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10f).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private List<string> getRandomList()
        {
            Random randVal = new Random();
            int rangeLimit = (emojiList.Count - 1);
            int randomIndex = randVal.Next(0, rangeLimit);
            return emojiList[randomIndex];
        }

        private void SetUpGame()
        {


            Random random = new Random();
            List<string> gameList = getRandomList();

            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    //Get a random index
                    int index = random.Next(gameList.Count);
                    string nextEmoji = gameList[index];
                    textBlock.Text = nextEmoji;
                    gameList.RemoveAt(index);
                }
            }

            // start timer
            timer.Start();
            //reset values
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock; 
            
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }

            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }

            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
