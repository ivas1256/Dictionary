using Dictionary.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace Dictionary.Data.Repository
{
    class WordRus_Repository : Repository<WordRus>
    {
        public WordRus_Repository(WordsContext dbContext) : base(dbContext)
        {
        }

        public List<WordRus> GetAllWithTranslations()
        {
            return ((WordsContext)dbContext).WordRus.Include("Translations").ToList();
        }

        public WordRus GetForEditor(int id)
        {
            return ((WordsContext)dbContext).WordRus
                .Include("Translations")
                //.Include("WordRusToRelated")
                .Include("WordRusToRelated.RelatedWord.WordRus")
                .Where(x => x.Id == id).FirstOrDefault();
        }

        public List<WordRus> FindWithTranslations(Expression<Func<WordRus, bool>> predicte)
        {
            return ((WordsContext)dbContext).WordRus.Include("Translations").Where(predicte).ToList();
        }
    }
}
