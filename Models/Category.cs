using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Models
{
    public enum CategoryType
    {
        Children,
        Cooking,
        History,
        IT,
        Fiction,
        NonFiction,
        Science,
        Sefhelp,
        Software,
        Stories,
        YoungAdult
    }
    public class Category
    {
        [Key]
        public int CatID { get; set; }

        [Required]
        [EnumDataType(typeof(CategoryType))]
        public CategoryType CatName { get; set; }

        [Required]
        public int NoBooks { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}
