using LibrarySystemDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
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
            return _context.Librarians.FirstOrDefault(n => n.LFName == name || n.LLName == name || n.LUserName == name);
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

        public bool Add(string FName, string LName, string email, string UName, string pass)
        {
            try
            {
                var librarian = new Librarian { LFName = FName, LLName = LName, LEmail = email, LUserName = UName, LPassword = pass };

                _context.Librarians.Add(librarian);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
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

        public int AdminAuthentication(string AdminUserName, string AdminPass)
        {
            var librarian = GetLibrarianByName(AdminUserName);
            if (librarian != null)
            {
                if (librarian.LPassword == AdminPass)
                {

                    return 1; //successful login
                }

                else
                {
                    return 2; //account found but wrong pass
                }
            }

            else
            {
                //call sign up function 
                return 3; //wrong username 
            }
        }
    }
}
