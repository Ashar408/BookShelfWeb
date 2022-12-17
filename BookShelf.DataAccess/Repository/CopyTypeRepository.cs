using BookShelf.DataAccess.Repository.IRepository;
using BookShelf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShelf.DataAccess.Repository
{
    public class CopyTypeRepository : Repository<CopyType>, ICopyTypeRepository
    {
        private ApplicationDbContext _db;
        public CopyTypeRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public void Update(CopyType obj)
        {
            _db.CopyTypes.Update(obj);
        }
    }
}
