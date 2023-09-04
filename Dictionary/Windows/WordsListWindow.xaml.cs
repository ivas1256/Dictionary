using Dictionary.Data.Model;
using Dictionary.Data.Repository;
using NLog;
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
using System.Windows.Shapes;

namespace Dictionary.Windows
{
    /// <summary>
    /// Interaction logic for WordsListWindow.xaml
    /// </summary>
    public partial class WordsListWindow : Window
    {
        public WordRus SelectedWord { get; set; }

        Logger logger = LogManager.GetCurrentClassLogger();
        List<WordRus> wordsToShow;
        List<int> wordIdsToHide;
        public WordsListWindow(List<WordRus> wordsToShow, List<int> wordIdsToHide)
        {
            this.wordsToShow = wordsToShow;
            this.wordIdsToHide = wordIdsToHide;
            InitializeComponent();
        }

        WordsDB db;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (wordsToShow == null)
                {
                    db = new WordsDB(new WordsContext());
                    lb_words.ItemsSource = db.RusWords.GetAll().Where(x => !wordIdsToHide.Contains(x.Id)).ToList();
                    btn_edit.Visibility = Visibility.Collapsed;
                }
                else
                {
                    lb_words.ItemsSource = wordsToShow;
                    btn_select.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageProvider.Error(ex, "Не удалось загрузить список");
                logger.Error(ex);
            }
        }

        private void lb_words_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var word = lb_words.SelectedItem as WordRus;
            if (word == null)
                return;

            if (wordsToShow == null)
            {                
                db.Dispose();

                SelectedWord = word;
                Close();
            }
            else
            {
                var w = new WordCardWindow(word.Id);
                w.Show();
            }
        }

        private void btn_select_Click(object sender, RoutedEventArgs e)
        {
            lb_words_MouseDoubleClick(null, null);
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var word = lb_words.SelectedItem as WordRus;
            if (word == null)
                return;

            var w = new WordEditorWindow(EditAction.Update, word.Id);            
            w.ShowDialog();
        }

        private void btn_openCard_Click(object sender, RoutedEventArgs e)
        {
            var word = lb_words.SelectedItem as WordRus;
            if (word == null)
                return;

            var w = new WordCardWindow(word.Id);
            w.Show();
        }
    }
}
