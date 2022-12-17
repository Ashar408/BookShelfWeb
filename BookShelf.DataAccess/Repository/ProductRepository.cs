using BookShelf.DataAccess.Repository.IRepository;
using BookShelf.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShelf.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }


        public void Update(Product obj)
        {
            var objfromDb = _db.Products.FirstOrDefault(u=>u.Id==obj.Id);
            if (objfromDb != null)
            {
                objfromDb.Title = obj.Title;
                objfromDb.Description = obj.Description;
                objfromDb.ISBN = obj.ISBN;
                objfromDb.Author = obj.Author;
                objfromDb.ListPrice = obj.ListPrice;
                objfromDb.Price = obj.Price;
                objfromDb.Price50 = obj.Price50;
                objfromDb.Price100 = obj.Price100;
                objfromDb.CategoryId = obj.CategoryId;
                objfromDb.CopyTypeId = obj.CopyTypeId;
                if (obj.ImageUrl != null)
                {
                    objfromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
