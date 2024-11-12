using LibrarySystemDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Repositories
{
    public class LibrarianRepo
    {
        private readonly ApplicationDBContext _context;
        public LibrarianRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Librarian> GetAll()
        {
            return _context.Librarians.ToList();
        }

        public Librarian GetLibrarianByID(int ID)
        {
            return _context.Librarians.Find(ID);
        }

        public Librarian GetLibrarianByName(string name)
        {
            return _context.Librarians.Find(name);
        }

        public void Update(string name)
        {
            var librarian = GetLibrarianByName(name);
            if (librarian != null)
            {
                _context.Librarians.Update(librarian);
                _context.SaveChanges();
            }
        }

        public void Add(Librarian librarians)
        {
            _context.Librarians.Add(librarians);
            _context.SaveChanges();
        }

        public void Delete(int ID)
        {
            var librarian = GetLibrarianByID(ID);
            if (librarian != null)
            {
                _context.Librarians.Remove(librarian);
                _context.SaveChanges();
            }
        }
    }
}
