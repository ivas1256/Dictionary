using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dictionary.Data.Repository
{
    /// <summary>
    /// it's IUnitOfWork as repository pattern tell's
    /// </summary>
    interface IUnitOfProductsWork : IDisposable
    {
        void Complete();
    }
}
