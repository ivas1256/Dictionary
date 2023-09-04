using Dictionary.Data.Model;
using Dictionary.Data.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Windows.ViewModel
{
    class WordsViewModel
    {
        ObservableCollection<WordRus> originalCollection;
        public ObservableCollection<WordRus> Words
        {
            get
            {
                var result = new ObservableCollection<WordRus>();
                foreach (var word in originalCollection)
                {
                    if (currLangId == 1)
                        word.CurrWordText = word.Text;
                    else
                    {
                        var tmp = word.Translations.Where(x => x.LanguageId == currLangId).FirstOrDefault();
                        word.CurrWordText = tmp?.Text;
                    }

                    if (!string.IsNullOrEmpty(word.CurrWordText))
                        result.Add(word);
                }

                return result;
            }
        }
        WordsDB db;

        public WordsViewModel(WordsDB db)
        {
            this.db = db;
            LoadWords();
        }

        void LoadWords()
        {
            originalCollection = new ObservableCollection<WordRus>();

            var result = new List<WordRus>();
            result.AddRange(db.RusWords.GetAllWithTranslations());
            foreach (var r in result)
                originalCollection.Add(r);
        }

        public void UpdateWords()
        {
            LoadWords();
        }

        int currLangId;
        public void Filter(int langId)
        {
            currLangId = langId;
        }
    }
}
