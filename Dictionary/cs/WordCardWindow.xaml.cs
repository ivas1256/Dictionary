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
    /// Interaction logic for WordCardWindow.xaml
    /// </summary>
    public partial class WordCardWindow : Window
    {
        int wordId;
        Logger logger = LogManager.GetCurrentClassLogger();
        public WordCardWindow(int wordId)
        {
            this.wordId = wordId;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var db = new WordsDB(new Data.Model.WordsContext());
                DataContext = db.RusWords.GetForEditor(wordId);
            }
            catch (Exception ex)
            {
                MessageProvider.Error(ex, "Не удалось загрузить карточку слова");
                logger.Error(ex);
            }
        }

        private void btn_openCard_Click(object sender, RoutedEventArgs e)
        {
            var word = lb_relatedWords.SelectedItem as WordRelated;
            if (word == null)
                return;

            var w = new WordCardWindow(word.WordRusId);
            w.Show();
        }

        private void btn_edit_Click(object sender, RoutedEventArgs e)
        {
            var word = lb_relatedWords.SelectedItem as WordRelated;
            if (word == null)
                return;

            var w = new WordEditorWindow(EditAction.Update, word.WordRusId);
            w.ShowDialog();
        }
    }
}
