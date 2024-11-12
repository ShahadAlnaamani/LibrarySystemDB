using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Models
{
    public class Librarian
    {
        [Key]
        public int LID { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string LUserName { get; set; }
        [Required]
        public string LPassword { get; set; }

        [Required]
        public string LEmail { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string LFName { get; set; }

        [StringLength(10, MinimumLength = 3)]
        public string LLName { get; set; }



    }
}
