using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Dictionary.Data.Model
{
    [MetadataType(typeof(WordTranslationMetadata))]
    public partial class WordTranslation : BaseDataModel
    {
        
    }

    internal sealed class WordTranslationMetadata
    {
        [Display(Name = "Перевод")]
        public string Text { get; set; }
    }
}
