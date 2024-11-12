using LibrarySystemDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Repositories
{
    public class CategoriesRepo
    {
        private readonly ApplicationDBContext _context;
        public CategoriesRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Category> GetAll()
        {
            return _context.Categories.ToList();
        }

        public Category GetCategoryByID(int ID)
        {
            return _context.Categories.Find(ID);
        }

        public Category GetCategoryByName(string name)
        {
            return _context.Categories.Find(name);
        }

        public void Update(string name)
        {
            var category = GetCategoryByName(name);
            if (category != null)
            {
                _context.Categories.Update(category);
                _context.SaveChanges();
            }
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public void Delete(int ID)
        {
            var category = GetCategoryByID(ID);
            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }
        }

        public int GetTotalBooksPerCategoryName(string name)
        {
            var category = GetCategoryByName(name);
            if (category != null)
            {
                return category.NoBooks;
            }
            else { return 0; }
        }
    }
}
