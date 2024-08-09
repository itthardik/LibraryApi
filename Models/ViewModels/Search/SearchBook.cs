using System.ComponentModel.DataAnnotations;

namespace LMS2.Models.ViewModels.Search
{
    /// <summary>
    /// search book class
    /// </summary>
    public class SearchBook
    {
        /// <summary>
        /// Title has limit upto 300
        /// </summary>
        [StringLength(300)]
        public string? Title { get; set; }
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
        [StringLength(300)]
        public string? PublisherName { get; set; }
    }
}
