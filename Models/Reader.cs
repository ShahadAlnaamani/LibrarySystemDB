using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Models
{
    public enum GenderType
    {
        Male, 
        Female
    }
    public class Reader
    {
        [Key]
        public int RID { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string RFName { get; set; }

        [StringLength(10, MinimumLength = 3)]
        public string RLName { get; set; }

        [Required]
        public string REmail { get; set; }

        public GenderType RGender { get; set; }

        [Required]
        [StringLength(8)]
        public string RPhoneNo { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 3)]
        public string RUserName { get; set; }

        [Required]
        public string Password { get; set; }

        public virtual ICollection<Borrow> Borrows { get; set; } 
    }
}
