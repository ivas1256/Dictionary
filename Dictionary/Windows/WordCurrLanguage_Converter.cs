using Dictionary.Data.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Dictionary.Windows
{
    class WordCurrLanguage_Converter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            int langId = (int)values[1];
            langId++;

            switch (langId)
            {
                case 1:
                    return ((WordRus)values[0]).Text;
                default:
                    return ((WordRus)values[0]).Translations.Where(x => x.LanguageId == langId).FirstOrDefault()?.Text;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
