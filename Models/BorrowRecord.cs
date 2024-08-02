using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LMS2.Models
{
    /// <summary>
    /// BorrowRecord Model
    /// </summary>
    public class BorrowRecord
    {
        /// <summary>
        /// Primary key with auto increment
        /// </summary>
        [Key]
        public int Id { get; private set; }
        /// <summary>
        /// Book Id is required and FK with Book
        /// </summary>
        [Required]
        [ForeignKey("Book")]
        public int BookId { get; set; }
        /// <summary>
        /// Member Id is required and FK with Member
        /// </summary>
        [Required]
        [ForeignKey("Member")]
        public int MemberId { get; set; }
        /// <summary>
        /// BorrowDate is Required
        /// </summary>
        [Required]
        public DateTime BorrowDate { get; set; }
        /// <summary>
        /// DueDate is Required
        /// </summary>
        [Required]
        public DateTime DueDate { get; set; }
        /// <summary>
        /// ReturnDate is Not Required
        /// </summary>
        public DateTime? ReturnDate { get; set; }
        /// <summary>
        /// is Deleted mark
        /// </summary>
        public bool IsDeleted { get; set; } = false;
        /// <summary>
        /// Penalty Amount by default 0
        /// </summary>
        public int PenaltyAmount { get; set; } = 0;
        /// <summary>
        /// Created Date is auto set as current dateTime
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        /// <summary>
        /// Penalty Status
        /// </summary>
        public bool IsPenaltyPaid { get; set; } = false;
        /// <summary>
        /// store the data of Book and not required field
        /// </summary>
        public Book? Book { get; set; }
        /// <summary>
        /// store the data of Book and not required field
        /// </summary>
        public Member? Member { get; set; }

    }
}
