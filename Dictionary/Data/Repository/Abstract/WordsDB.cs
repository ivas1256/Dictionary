using Dictionary.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Data.Repository
{
    class WordsDB : IUnitOfProductsWork
    {
        readonly WordsContext context;
        public WordsDB(WordsContext _context)
        {
            context = _context;

            RusWords = new WordRus_Repository(context);
            WordTranslations = new WordTranslation_Repository(context);
            RelatedWordNodes = new WordRelated_Repository(context);
            Languages = new Language_Repository(context);
            WordsRusToRelated = new WordRusToRelated_Repository(context);
        }

        public WordRus_Repository RusWords { get; private set; }
        public WordTranslation_Repository WordTranslations { get; private set; }
        public WordRelated_Repository RelatedWordNodes { get; private set; }
        public Language_Repository Languages { get; private set; }
        public WordRusToRelated_Repository WordsRusToRelated { get; private set; }

        public Database Database
        {
            get
            {
                return context.Database;
            }
        }

        public void Sql(string sql, params object[] args)
        {
            context.Database.ExecuteSqlCommand(sql, args);
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void Complete()
        {
            context.SaveChanges();
        }
    }
}
