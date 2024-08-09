using System.ComponentModel.DataAnnotations;

namespace LMS2.Models.ViewModels.Request
{
    /// <summary>
    /// model for request borrowRecord
    /// </summary>
    public class RequestBorrowRecord
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
        /// <summary>
        /// Penalty paid Statuts
        /// </summary>
        public bool? IsPenaltyPaid { get; set; }
    }
}