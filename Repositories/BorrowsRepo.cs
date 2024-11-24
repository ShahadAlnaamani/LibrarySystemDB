using LibrarySystemDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
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

            public List<Borrow> GetBorrowByReaderID(int ID)
            {
                return _context.Borrows.Where(r => r.BRID == ID).ToList();
            }

        public Borrow GetBorrow(int RID, int BID)
        {
            var borrow = (Borrow)_context.Borrows.Include(r => r.Books).Include(r => r.Readers).FirstOrDefault(r => r.BRID == RID && r.BBID == BID && r.IsReturned == IsReturnedType.NotReturned);
            return borrow;
        }

        //public void Update(int ID)
        //    {
        //        var borrow = GetBorrowByReaderID(ID);
        //        if (borrow != null)
        //        {
        //            _context.Borrows.Update(borrow);
        //            _context.SaveChanges();
        //        }
        //    }

            public bool Add(int BID, int ReaderID, ApplicationDBContext applicationDBContext)
            {
                BooksRepo book = new BooksRepo(applicationDBContext);
                var ThisBook = book.GetBookByID(BID);

            if (ThisBook != null)
            {
                DateOnly Return = DateOnly.FromDateTime(DateTime.Now);
                Return = Return.AddDays(ThisBook.BorrowPeriod);

                //Adding Borrow 
                var borrow = new Borrow { BBID = ThisBook.BookID, BRID = ReaderID, BorrowedDate = DateTime.Now, PredictedReturn = Return, ActualReturn = null, Rating = null, IsReturned = IsReturnedType.NotReturned };
                _context.Borrows.Add(borrow);

                //Updating book 
                ThisBook.BorrowedCopies = ThisBook.BorrowedCopies + 1;
                _context.Books.Update(ThisBook);

                _context.SaveChanges();

                return true;
            }

            else { return false; } // ie failed
            }

            public bool Return(int ID, int BID, ApplicationDBContext applicationDbContext, RatingTypes Rating)
            {
                var borrow = GetBorrow(ID, BID);
            if (borrow != null)
            {
                //Return book
                borrow.IsReturned = IsReturnedType.Returned;
                borrow.ActualReturn = DateOnly.FromDateTime(DateTime.Now);
                borrow.Rating = Rating;

                //Update book
                BooksRepo book = new BooksRepo(applicationDbContext);
                var ThisBook = book.GetBookByID(BID);
                ThisBook.BorrowedCopies--;

                _context.Books.Update(ThisBook);
                _context.Borrows.Update(borrow);
                _context.SaveChanges();
                return true;
            }

            else
            { return false;  }
            }

        //public void Delete(int ID)
        //    {
        //        var borrow = GetBorrowByReaderID(ID);
        //        if (borrow != null)
        //        {
        //            _context.Borrows.Remove(borrow);
        //            _context.SaveChanges();
        //        }
        //    }

            public int GetTotalBorrowedBooks()
            {
                   IsReturnedType notreturned = IsReturnedType.NotReturned;
                   return _context.Borrows.Where(g => g.IsReturned == notreturned).Count();
            }

        public int GetTotalBorrowedBooksByReader(Reader reader)
        {
            IsReturnedType returned = IsReturnedType.Returned;
            return _context.Borrows.Where(g => g.BRID == reader.RID && g.IsReturned == returned ).Count();
        }

        public List<Borrow> OverdueFinder(Reader reader)
        {
            List<Borrow> overdue = new List<Borrow>();
           IsReturnedType notreturned = IsReturnedType.NotReturned;
            foreach (Borrow b in _context.Borrows.Where(r => r.BRID == reader.RID))
            { 
                if ((b.IsReturned == notreturned) && (b.PredictedReturn < (DateOnly.FromDateTime(DateTime.Now))))
                { 
                    overdue.Add(b);
                }
            }

            return overdue;
        }
    }
}
