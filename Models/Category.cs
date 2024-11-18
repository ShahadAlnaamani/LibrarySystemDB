using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Models
{

    public class Category
    {
        [Key]
        public int CatID { get; set; }

        [Required]
        [NotNull]
        public string CategoryTypes { get; set; }
 

        [Required]
        public int NoBooks { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
