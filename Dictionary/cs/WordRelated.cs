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
    
    public partial class WordRelated
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WordRelated()
        {
            this.WordRusToRelated = new HashSet<WordRusToRelated>();
        }
    
        public int Id { get; set; }
        public int WordRusId { get; set; }
    
        public virtual WordRus WordRus { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WordRusToRelated> WordRusToRelated { get; set; }
    }
}
