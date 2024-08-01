using System.ComponentModel.DataAnnotations;

namespace LMS2.Models.ViewModels
{
    /// <summary>
    /// model for input borrowRecord
    /// </summary>
    public class InputBorrowRecord
    {
        /// <summary>
        /// Book's ID
        /// </summary>
        public int? BookId { get; set; }
        /// <summary>
        /// Member's ID
        /// </summary>
        public int? MemberId { get; set; }
        /// <summary>
        /// Borrow Date
        /// </summary>
        public DateTime? BorrowDate { get; set; }
        /// <summary>
        /// Due Date
        /// </summary>
        public DateTime? DueDate { get; set; }
        /// <summary>
        /// Return Date
        /// </summary>
        public DateTime? ReturnDate { get; set; }
    }
}