using Dictionary.Data.Model;
using Dictionary.Data.Repository;
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
using System.Windows.Shapes;

namespace Dictionary.Windows
{
    public enum EditAction { Add, Update };
    /// <summary>
    /// Interaction logic for WordEditorWindow.xaml
    /// </summary>
    public partial class WordEditorWindow : Window
    {
        int updatedId;
        EditAction editAction;

        Logger logger = LogManager.GetCurrentClassLogger();
        public WordEditorWindow(EditAction editAction, int updatedId = -1)
        {
            this.updatedId = updatedId;
            this.editAction = editAction;
            InitializeComponent();
        }

        internal WordsDB context;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (context == null)
                context = new WordsDB(new WordsContext());
            switch (editAction)
            {
                case EditAction.Add:
                    DataContext = new WordRus();
                    break;
                case EditAction.Update:
                    Title = "Редактирование слова";
                    btn_ok.Content = "Изменить";
                    DataContext = context.RusWords.GetForEditor(updatedId);
                    break;
            }
        }

        private void btn_addRelatedWord_Click(object sender, RoutedEventArgs e)
        {
            var word = DataContext as WordRus;
            if (word == null)
                return;

            var hiddenIds = new List<int> { word.Id };
            hiddenIds.AddRange(word.RelatedWords.Select(x => x.WordRusId));

            var w = new WordsListWindow(null, hiddenIds);
            w.Title = $"Выбрать смежное слово для {word.Text}";
            w.ShowDialog();

            try
            {
                if (w.SelectedWord != null)
                {
                    var targetWord = context.RusWords.Get(w.SelectedWord.Id);
                    word.WordRusToRelated.Add(new WordRusToRelated()
                    {
                        RelatedWord = new WordRelated()
                        {
                            WordRus = targetWord
                        }
                    });

                    targetWord.WordRusToRelated.Add(new WordRusToRelated()
                    {
                        RelatedWord = new WordRelated()
                        {
                            WordRus = word
                        }
                    });

                    word.PropChanged("RelatedWords");
                }
            }
            catch (Exception ex)
            {
                MessageProvider.UnknownError(ex);
                logger.Error(ex);
            }
        }

        private void btn_ok_Click(object sender, RoutedEventArgs e)
        {
            var word = DataContext as WordRus;
            if (word == null)
                return;

            if (!word.IsValid(this))
                return;

            try
            {
                switch (editAction)
                {
                    case EditAction.Add:
                        context.RusWords.Add(word);
                        context.Complete();
                        break;
                    case EditAction.Update:
                        context.Complete();
                        break;
                }

                Close();
            }
            catch (Exception ex)
            {
                //todo entityvalidatein erros
                MessageProvider.Error(ex, "Не удалось сохранить запись");
                logger.Error(ex);
            }
        }

        private void btn_deleteRelated_Click(object sender, RoutedEventArgs e)
        {
            var word = DataContext as WordRus;
            if (word == null)
                return;

            var related = lb_relatedWords.SelectedItem as WordRelated;
            if (related == null)
                return;

            try
            {
                if (!MessageProvider.Confirm($"Действительно удалить слово {related.WordRus.Text} из смежных?"))
                    return;

                var toDelete = context.WordsRusToRelated
                    .Find(x => x.RelatedWordId == related.WordRusId || x.RelatedWordId == word.Id);
                context.WordsRusToRelated.RemoveRange(toDelete);

                var toDelete2 = context.RelatedWordNodes
                    .Find(x => x.WordRusId == related.WordRusId || x.WordRusId == word.Id);
                context.RelatedWordNodes.RemoveRange(toDelete2);

                context.Complete();
                word.PropChanged("RelatedWords");
            }
            catch (Exception ex)
            {
                MessageProvider.Error(ex, "Не удалось удалить слово");
                logger.Error(ex);
            }
        }

        private void btn_cansel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btn_openCard_Click(object sender, RoutedEventArgs e)
        {
            var related = lb_relatedWords.SelectedItem as WordRelated;
            if (related == null)
                return;

            var w = new WordCardWindow(related.WordRusId);
            w.Show();
        }

        private void lb_relatedWords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btn_openCard_Click(null, null);
        }

        private void lb_relatedWords_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Delete)
                return;

            btn_deleteRelated_Click(null, null);
        }
    }
}
