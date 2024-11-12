using LibrarySystemDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Repositories
{
    public class ReadersRepo
    {
        private readonly ApplicationDBContext _context;
        public ReadersRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public List<Reader> GetAll()
        {
            return _context.Readers.ToList();
        }

        public Reader GetReaderByID(int ID)
        {
            return _context.Readers.Find(ID);
        }

        public Reader GetReaderByName(string name)
        {
            return _context.Readers.Find(name);
        }

        public void Update(string name)
        {
            var reader = GetReaderByName(name);
            if (reader != null)
            {
                _context.Readers.Update(reader);
                _context.SaveChanges();
            }
        }

        public void Add(Reader reader)
        {
            _context.Readers.Add(reader);
            _context.SaveChanges();
        }

        public void Delete(int ID)
        {
            var reader = GetReaderByID(ID);
            if (reader != null)
            {
                _context.Readers.Remove(reader);
                _context.SaveChanges();
            }
        }

        public int CountByGender(GenderType gender)
        {
           return _context.Readers.Where(g=> g.RGender == gender).Count();
        }
    }
}
