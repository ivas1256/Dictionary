using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dictionary.Data.Model
{
    [MetadataType(typeof(WordRusMetadata))]
    public partial class WordRus : BaseDataModel, INotifyPropertyChanged
    {
        public string CurrWordText { get; set; }

        public string Eng
        {
            get
            {
                var check = Translations.Where(x => x.LanguageId == 2).FirstOrDefault();
                if (check == null)
                {
                    var engObj = new WordTranslation() { LanguageId = 2 };
                    Translations.Add(engObj);
                    return engObj.Text;
                }
                else
                    return check.Text;
            }
            set
            {
                var check = Translations.Where(x => x.LanguageId == 2).FirstOrDefault();
                check.Text = value;

                PropChanged("Eng");
            }
        }
        public string Esp
        {
            get
            {
                var check = Translations.Where(x => x.LanguageId == 3).FirstOrDefault();
                if (check == null)
                {
                    var espObj = new WordTranslation() { LanguageId = 3 };
                    Translations.Add(espObj);
                    return espObj.Text;
                }
                else
                    return check.Text;
            }
            set
            {
                var check = Translations.Where(x => x.LanguageId == 3).FirstOrDefault();
                check.Text = value;

                PropChanged("Esp");
            }
        }

        public List<WordRelated> RelatedWords
        {
            get
            {
                return WordRusToRelated.Select(x => x.RelatedWord).ToList();
            }
        }

        public bool IsValid(DependencyObject validateControl)
        {
            var result = ClassValid(validateControl);
            return result;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void PropChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

    internal sealed class WordRusMetadata
    {
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Оригинал на русском")]
        public string Text { get; set; }
    }
}
