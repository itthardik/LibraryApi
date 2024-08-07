using System.ComponentModel.DataAnnotations;

namespace LMS2.Models.ViewModels
{
    /// <summary>
    /// search member class
    /// </summary>
    public class SearchMember
    {
        /// <summary>
        /// Name and has limit upto 300
        /// </summary>
        [StringLength(100)]
        public string? Name { get; set; }
        /// <summary>
        /// Email and has limit upto 100 and regx to validate
        /// </summary>
        [StringLength(100)]
        public string? Email { get; set; }
        /// <summary>
        /// MobileNumber
        /// </summary>
        [StringLength(100)]
        public string? MobileNumber { get; set; }
        /// <summary>
        /// Pincode and has regx to validate
        /// </summary>
        [StringLength(100)]
        public string? Pincode { get; set; }
    }
}
