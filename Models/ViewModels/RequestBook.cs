using System.ComponentModel.DataAnnotations;

namespace LMS2.Models.ViewModels
{
    /// <summary>
    /// model for request book
    /// </summary>
    public class RequestBook
    {
        /// <summary>
        /// Title has limit upto 300
        /// </summary>
        [StringLength(300)]
        public string? Title { get; set; }
        /// <summary>
        /// Description 
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Author name has limit upto 300
        /// </summary>
        [StringLength(300)]
        public string? AuthorName { get; set; }
        /// <summary>
        /// Genre has limit upto 300
        /// </summary>
        [StringLength(300)]
        public string? Genre { get; set; }
        /// <summary>
        /// Publisher Name has limit upto 300
        /// </summary>
        public string? PublisherName { get; set; }
        /// <summary>
        /// Publisher Desc 
        /// </summary>
        public string? PubliserDescription { get; set; }
        /// <summary>
        /// Price cannot be negative
        /// </summary>
        [Range(0, int.MaxValue)]
        public int? Price { get; set; }
        /// <summary>
        /// current stock cannot be negative
        /// </summary>
        [Range(0, int.MaxValue)]
        public int? CurrentStock { get; set; }
    }
}
