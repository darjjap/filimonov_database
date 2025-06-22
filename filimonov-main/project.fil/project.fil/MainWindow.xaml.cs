using project.fil.Pages;
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
using project.fil.Pages;

namespace project.fil
{
    public partial class MainWindow : Window
    {
        public void SetSelectedWord(string word)
        {
            _selectedWord = word;
        }

        public void SetDisplayWord(string displayWord)
        {
            _displayWord = displayWord;
        }

        public readonly Dictionary<string, List<string>> _wordCategories = new Dictionary<string, List<string>>
        {
            { "Природа", new List<string> { "дерево", "папоротник", "гора", "река" } },
            { "Животные", new List<string> { "кот", "собака", "слон", "тигр" } },
            { "Техника", new List<string> { "машина", "компьютер", "телефон", "робот" } },
            { "Еда", new List<string> { "яблоко", "хлеб", "молоко", "мясо" } },
            { "Спорт", new List<string> { "футбол", "баскетбол", "теннис", "бокс" } }
        };

        public string _selectedWord;
        public string _displayWord;
        public int _wrongGuesses;
        public List<string> _guessedLetters = new List<string>();
        public Dictionary<string, (int Wins, int Losses)> _leaderboard = new Dictionary<string, (int Wins, int Losses)>();
        public string _nickname;

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new SignInPage());
        }
        public void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрытие окна приложения
            this.Close();
        }
        public void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _nickname = NicknameTextBox.Text;
            if (string.IsNullOrEmpty(_nickname))
            {
                MessageBox.Show("Введите ваш никнейм!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedCategory = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (string.IsNullOrEmpty(selectedCategory) || !_wordCategories.ContainsKey(selectedCategory))
            {
                MessageBox.Show("Выберите категорию!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            InitializeGame(selectedCategory);
        }

        public void InitializeGame(string category)
        {
            _selectedWord = GetRandomWord(category);
            _displayWord = new string('_', _selectedWord.Length);
            _wrongGuesses = 0;
            _guessedLetters.Clear();

            WordDisplay.Text = _displayWord;
            HangmanImage.Source = new BitmapImage(new Uri("/fail0.jpg", UriKind.Relative));
            MessageTextBlock.Text = "";
            GuessButton.IsEnabled = true;

            LetterTextBox.Clear();
            UpdateLeaderboardDisplay();
        }

        public string GetRandomWord(string category)
        {
            Random rand = new Random();
            var words = _wordCategories[category];
            return words[rand.Next(words.Count)];
        }

        public void GuessButton_Click(object sender, RoutedEventArgs e)
        {
            string letter = LetterTextBox.Text.ToLower();
            if (_guessedLetters.Contains(letter) || letter.Length != 1)
            {
                MessageTextBlock.Text = "Вы уже пробовали эту букву или ввели неправильный символ.";
                LetterTextBox.Clear();
                return;
            }

            _guessedLetters.Add(letter);

            try
            {
                if (_selectedWord.Contains(letter))
                {
                    _displayWord = UpdateDisplayWord(letter);
                    WordDisplay.Text = _displayWord;
                    MessageTextBlock.Text = "Правильно!";
                }
                else
                {
                    _wrongGuesses++;
                    HangmanImage.Source = new BitmapImage(new Uri($"fail{_wrongGuesses}.jpg", UriKind.Relative));
                    MessageTextBlock.Text = "Неверно!";
                }

                LetterTextBox.Clear();

                if (_displayWord == _selectedWord)
                {
                    MessageTextBlock.Text = "Вы выиграли!";
                    UpdateLeaderboard(true); // игрок выиграл
                    GuessButton.IsEnabled = false;
                }
                else if (_wrongGuesses >= 12)
                {
                    MessageTextBlock.Text = $"Вы проиграли! Загаданное слово: {_selectedWord}";
                    UpdateLeaderboard(false); // игрок проиграл
                    GuessButton.IsEnabled = false;
                }

                UpdateLeaderboardDisplay();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string UpdateDisplayWord(string letter)
        {
            char[] display = _displayWord.ToCharArray();
            for (int i = 0; i < _selectedWord.Length; i++)
            {
                if (_selectedWord[i].ToString() == letter)
                    display[i] = letter[0];
            }
            return new string(display);
        }

        public void UpdateLeaderboard(bool isWin)
        {
            if (!_leaderboard.ContainsKey(_nickname))
            {
                _leaderboard[_nickname] = (0, 0); // Начинаем с нуля
            }

            var (Wins, Losses) = _leaderboard[_nickname];
            if (isWin)
            {
                Wins++;
            }
            else
            {
                Losses++;
            }

            _leaderboard[_nickname] = (Wins, Losses);
        }

        public void UpdateLeaderboardDisplay()
        {
            Leaderboard.Items.Clear();
            foreach (var player in _leaderboard)
            {
                var (Wins, Losses) = player.Value;
                Leaderboard.Items.Add($"{player.Key} - П/П: {Wins}/{Losses}"); // выводим количество побед и поражений
            }
        }

        public void ReplayButton_Click(object sender, RoutedEventArgs e)
        {
            // Переиграть, сохраняя данные в Leaderboard
            var selectedCategory = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (!string.IsNullOrEmpty(selectedCategory))
            {
                InitializeGame(selectedCategory);
            }
        }
    }
}