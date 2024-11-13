using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemDB.Models
{
    public enum IsReturnedType
    {
        Returned,
        NotReturned
    }

    public enum RatingTypes
    {
        Excellent,
        Great,
        Good,
        Okay,
        Bad,
        VeryBad
    }

    [PrimaryKey(nameof(BBID), nameof(BRID), nameof(BorrowedDate))]


    public class Borrow
    {
        [ForeignKey("Books")]
        public int BBID { get; set; }
        public virtual Book Books { get; set; }


        [ForeignKey("Readers")]
        public int BRID { get; set; }
        public virtual Reader Readers { get; set; }

        public DateTime BorrowedDate { get; set; }

        public DateOnly PredictedReturn {  get; set; }


        [Required]
        [EnumDataType(typeof(RatingTypes))]
        public RatingTypes Rating { get; set; }

        public DateOnly? ActualReturn { get; set; }

        [Required]
        [EnumDataType(typeof(IsReturnedType))]
        [DefaultValue(IsReturnedType.NotReturned)]
        public IsReturnedType IsReturned  { get; set; }

    }
}
