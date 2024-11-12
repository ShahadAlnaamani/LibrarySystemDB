using LibrarySystemDB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Repositories
{
    public class BorrowsRepo
    {
            private readonly ApplicationDBContext _context;
            public BorrowsRepo(ApplicationDBContext context)
            {
                _context = context;
            }

            public List<Borrow> GetAll()
            {
                return _context.Borrows.ToList();
            }

            public Borrow GetBorrowByID(int ID)
            {
                return _context.Borrows.Find(ID);
            }

            public void Update(int ID)
            {
                var borrow = GetBorrowByID(ID);
                if (borrow != null)
                {
                    _context.Borrows.Update(borrow);
                    _context.SaveChanges();
                }
            }

            public void Add(Borrow borrow)
            {
                _context.Borrows.Add(borrow);
                _context.SaveChanges();
            }

            public void Delete(int ID)
            {
                var borrow = GetBorrowByID(ID);
                if (borrow != null)
                {
                    _context.Borrows.Remove(borrow);
                    _context.SaveChanges();
                }
            }

            public int GetTotalBorrowedBooks()
            {
                   IsReturnedType notreturned = IsReturnedType.NotReturned;
                   return _context.Borrows.Where(g => g.IsReturned == notreturned).Count();
            }
    }
}
