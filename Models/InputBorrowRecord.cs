using System.ComponentModel.DataAnnotations;

namespace LMS2.Models
{
    public class InputBorrowRecord
    {
        [Required]
        public int BookId { get; set; }
        [Required]
        public int MemberId { get; set; }
        [Required]
        public DateTime BorrowDate { get; set; }
        [Required]
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
    }
}