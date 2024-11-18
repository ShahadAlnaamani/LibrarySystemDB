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
            return _context.Categories.FirstOrDefault(n => n.CategoryTypes == name);
        }

        public int Update(string name, int catID)
        {
            var category = GetCategoryByID(catID);
            if (category != null)
            {
                var NameUsed = GetCategoryByName(name);
                if (NameUsed == null)
                {
                    category.CategoryTypes = name;
                    _context.Categories.Update(category);
                    _context.SaveChanges();
                    return 1; //complete 
                }

                else { return 2;  } //name already used 
            }

            else return 0; //category ID nopt valid  
        }

        public bool Add(string CatName)
        {
            var catname  = GetCategoryByName(CatName);
            if (catname == null)
            {
                var category = new Category { CategoryTypes = CatName, NoBooks = 0 };
                _context.Categories.Add(category);
                _context.SaveChanges();
                return true;
            }

            else { return false; }  
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
