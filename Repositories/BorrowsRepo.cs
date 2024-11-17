﻿using LibrarySystemDB.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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

            public List<Borrow> GetBorrowByReaderID(int ID)
            {
                return _context.Borrows.Where(r => r.BRID == ID).ToList();
            }

        public void Update(int ID)
            {
                Borrow borrow = GetBorrowByID(ID);
                if (borrow != null)
                {
                    _context.Borrows.Update(borrow);
                    _context.SaveChanges();
                }
            }

            public bool Add(int BID, int ReaderID, ApplicationDBContext applicationDBContext)
            {
                BooksRepo book = new BooksRepo(applicationDBContext);
                var ThisBook = book.GetBookByID(BID);

            if (ThisBook != null)
            {
                DateOnly Return = DateOnly.FromDateTime(DateTime.Now);
                Return.AddDays(ThisBook.BorrowPeriod);

                //Adding Borrow 
                var borrow = new Borrow { BBID = ThisBook.BookID, BRID = ReaderID, BorrowedDate = DateTime.Now, PredictedReturn = Return, ActualReturn = null, Rating = null, IsReturned = IsReturnedType.NotReturned };
                _context.Borrows.Update(borrow);

                //Updating book 
                ThisBook.BorrowedCopies = ThisBook.BorrowedCopies + 1;
                _context.Books.Update(ThisBook);

                _context.SaveChanges();
                return true;
            }

            else { return false; } // ie failed
            }

            public void Return(int ID)
            {
                var borrow = GetBorrowByID(ID);
                if (borrow != null)
                {
                    _context.Borrows.Update(borrow);
                    _context.SaveChanges();
                }
            }

        public void Delete(int ID)
            {
                var borrow = GetBorrowByReaderID(ID);
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
