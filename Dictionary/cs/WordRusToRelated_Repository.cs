using Dictionary.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Dictionary.Data.Repository
{
    class WordRusToRelated_Repository : Repository<WordRusToRelated>
    {
        public WordRusToRelated_Repository(WordsContext dbContext) : base(dbContext)
        {
        }
    }
}
