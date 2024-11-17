using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystemDB.Models;


namespace LibrarySystemDB.Repositories
{
    public class BooksRepo
    {
        private readonly ApplicationDBContext _context;
        public BooksRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Book> GetAll()
        {
            return _context.Books.ToList();
        }

        public List<Book> GetAllByAuthor(string Name)
        {
            return _context.Books.Where(i=>i.AuthFName == Name || i.AuthLName == Name).ToList();
        }

        public Book GetBookByTitle(string BookTitle)
        {
            return _context.Books.FirstOrDefault(b => b.Title == BookTitle);
        }

        public Book GetBookByID(int ID)
        {
            return _context.Books.Find(ID);
        }

        public void Update(string title)
        {
            var book = GetBookByTitle(title);
            if (book != null)
            {
                _context.Books.Update(book);
                _context.SaveChanges();
            }
        }

        public void Add(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public void Delete(int ID)
        {
            var book = GetBookByID(ID);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
            }
        }

        public decimal GetTotalPrice(Book book)
        {
            return _context.Books.Sum(e=> e.Price * e.TotalCopies);
        }

        public decimal GetMaxPrice(Book book) //taking the price as price per day
        {
            return _context.Books.Sum(e => e.Price * e.BorrowPeriod);
        }

        public int GetTotalBooksPerCategoryName(CategoryType catName)
        { 
            return _context.Categories.Where(c=> c.CatName == catName).Count();
        }
    }
}
