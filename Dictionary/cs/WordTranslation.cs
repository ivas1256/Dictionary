//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Dictionary.Data.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class WordTranslation
    {
        public int Id { get; set; }
        public int WordId { get; set; }
        public string Text { get; set; }
        public int LanguageId { get; set; }
    
        public virtual WordRus WordRus { get; set; }
        public virtual Language Language { get; set; }
    }
}
