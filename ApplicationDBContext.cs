using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrarySystemDB.Models;


namespace LibrarySystemDB
{
    public class ApplicationDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            options.UseSqlServer(" Data Source=(local); Initial Catalog=LibraryDB; Integrated Security=true; TrustServerCertificate=True ");
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Borrow> Borrows { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Reader> Readers { get; set; }


    }
}
