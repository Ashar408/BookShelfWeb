using BookShelf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShelf.DataAccess.Repository.IRepository
{
    public interface ICopyTypeRepository : IRepository<CopyType>
    {
        void Update(CopyType obj);
      
    }
}
