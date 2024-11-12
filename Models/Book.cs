using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Models
{
    public class Book
    {
        [Key]
        public int BookID { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public int BorrowPeriod { get; set; }

        [Required]
        [StringLength(10, MinimumLength =3)]
        public string AuthFName { get; set; }

        [StringLength(10, MinimumLength = 3)]
        public string AuthLName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int TotalCopies { get; set; }

        public int BorrowedCopies { get; set; }

        [ForeignKey("Categories")]
        public int BCID { get; set; } //Category ID 
        public virtual Category Categories { get; set; }


        public virtual ICollection<Borrow> Borrows { get; set;}
    }
}
