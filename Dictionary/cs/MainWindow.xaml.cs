using Dictionary.Data.Model;
using Dictionary.Data.Repository;
using Dictionary.Windows;
using Dictionary.Windows.ViewModel;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
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

namespace Dictionary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Logger logger = LogManager.GetCurrentClassLogger();
        WordsDB wordsContext;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                wordsContext = new WordsDB(new Data.Model.WordsContext());
                DataContext = new WordsViewModel(wordsContext);
                cb_language.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageProvider.Error(ex, "Не удалось подключиться к базе данных");
                logger.Error(ex);
            }
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            try
            {
                if (wordsContext == null)
                    return;

                DataContext = new WordsViewModel(wordsContext);
                cb_language_SelectionChanged(this, null);
            }
            catch (Exception ex)
            {
                MessageProvider.Error(ex, "Не удалось обновить список слов");
                logger.Error(ex);
            }
        }

        private void btn_addWord_Click(object sender, RoutedEventArgs e)
        {
            var w = new WordEditorWindow(EditAction.Add);
            w.context = wordsContext;
            w.ShowDialog();
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var word = lb_words.SelectedItem as WordRus;
            if (word == null)
                return;

            var w = new WordEditorWindow(EditAction.Update, word.Id);
            w.context = wordsContext;
            w.ShowDialog();
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            var word = lb_words.SelectedItem as WordRus;
            if (word == null)
                return;

            try
            {
                if (!MessageProvider.Confirm($"Действительно удалить слово {word.Text}?"))
                    return;

                int id = word.Id;

                wordsContext.RelatedWordNodes.RemoveRange(wordsContext.RelatedWordNodes.Find(x => x.WordRusId == id));
                wordsContext.WordsRusToRelated.RemoveRange(wordsContext.WordsRusToRelated.Find(x => x.WordRusId == id));

                wordsContext.WordTranslations.RemoveRange(wordsContext.WordTranslations.Find(x => x.WordId == id));
                wordsContext.RusWords.Remove(word);
                wordsContext.Complete();

                Window_Activated(null, null);
            }
            catch (Exception ex)
            {
                MessageProvider.Error(ex, "Не удалось удалить слово");
                logger.Error(ex);
            }
        }

        private void cb_language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                (DataContext as WordsViewModel)?.Filter(cb_language.SelectedIndex + 1);
                lb_words.GetBindingExpression(ListBox.ItemsSourceProperty).UpdateTarget();
            }
            catch (Exception ex)
            {
                MessageProvider.UnknownError(ex);
                logger.Error(ex);
            }
        }

        private void btn_openCard_Click(object sender, RoutedEventArgs e)
        {
            var word = lb_words.SelectedItem as WordRus;
            if (word == null)
                return;

            var w = new WordCardWindow(word.Id);
            w.Show();
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            var query = tb_searchQuery.Text;
            if (string.IsNullOrEmpty(query))
                return;

            try
            {
                var arr = query.Split(new string[] { "OR" }, StringSplitOptions.RemoveEmptyEntries);
                var result = new List<WordRus>();

                foreach(var a in arr)
                   result.AddRange(wordsContext.RusWords.Find(x => x.Text.Contains(a.Trim())));

                var w = new WordsListWindow(result, null);
                w.Title = $"Поиск по запросу: {query}";
                w.Show();
            }
            catch (Exception ex)
            {
                MessageProvider.UnknownError(ex);
                logger.Error(ex);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                wordsContext?.Dispose();
            }
            catch
            {
            }
        }

        private void lb_words_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var word = lb_words.SelectedItem as WordRus;
            if (word == null)
                return;

            var w = new WordCardWindow(word.Id);
            w.Show();
        }

        private void lb_words_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;

            btn_delete_Click(null, null);
        }

        private void tb_searchQuery_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            btn_search_Click(null, null);
        }
    }
}
