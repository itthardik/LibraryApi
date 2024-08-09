namespace LMS2.Models.ViewModels.Search
{
    /// <summary>
    /// Search class for borrow Records
    /// </summary>
    public class SearchBorrowRecords
    {
        /// <summary>
        /// BookID
        /// </summary>
        public int? BookId { get; set; }
        /// <summary>
        /// MemberID
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
        /// Book Name
        /// </summary>
        public string? BookName { get; set; }
        /// <summary>
        /// Author
        /// </summary>
        public string? Author { get; set; }
        /// <summary>
        /// Genre
        /// </summary>
        public string? Genre { get; set; }
        /// <summary>
        /// Publisher Name
        /// </summary>
        public string? PublisherName { get; set; }
        /// <summary>
        /// Member Name
        /// </summary>
        public string? MemberName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string? Email { get; set; }
        /// <summary>
        /// Mobile Number
        /// </summary>
        public int? MobileNumber { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Pincode
        /// </summary>
        public string? Pincode { get; set; }
    }
}
