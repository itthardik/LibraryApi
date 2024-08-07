using LMS2.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace LMS2.Models
{
    /// <summary>
    /// Book Model
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Primary key with auto increment
        /// </summary>
        [Key]
        public int? Id { get; private set; }
        /// <summary>
        /// Title is required and has limit upto 300
        /// </summary>
        [Required]
        [StringLength(300)]
        public string? Title { get; set; }
        /// <summary>
        /// Description is Required 
        /// </summary>
        [Required]
        public string? Description { get; set; }
        /// <summary>
        /// Author name is required and has limit upto 300
        /// </summary>
        [Required]
        [StringLength(300)]
        public string? AuthorName { get; set; }
        /// <summary>
        /// Genre is required and has limit upto 300
        /// </summary>
        [Required]
        [StringLength(300)]
        public string? Genre { get; set; }
        /// <summary>
        /// Publisher Name is required and has limit upto 300
        /// </summary>
        [Required]
        [StringLength(300)]
        public string? PublisherName { get; set; }
        /// <summary>
        /// Publisher Desc is required
        /// </summary>
        [Required]
        public string? PublisherDescription { get; set; }
        /// <summary>
        /// Price is required and cannot be negative
        /// </summary>
        [Required]
        [Range(0,int.MaxValue)]
        public int? Price { get; set; }
        /// <summary>
        /// current Stock is required and cannot be negative
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public int? CurrentStock { get; set; }
        /// <summary>
        /// is Deleted mark
        /// </summary>
        public bool IsDeleted { get; set; } = false;
        /// <summary>
        /// Created Date is auto set as current dateTime
        /// </summary>
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
    }
}
