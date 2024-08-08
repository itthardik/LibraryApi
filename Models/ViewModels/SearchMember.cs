using System.ComponentModel.DataAnnotations;

namespace LMS2.Models.ViewModels
{
    /// <summary>
    /// search member class
    /// </summary>
    public class SearchMember
    {
        /// <summary>
        /// Name
        /// </summary>
        [StringLength(100)]
        public string? Name { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [StringLength(100)]
        public string? Email { get; set; }
        /// <summary>
        /// MobileNumber
        /// </summary>
        [StringLength(100)]
        public string? MobileNumber { get; set; }
        /// <summary>
        /// Pincode 
        /// </summary>
        [StringLength(100)]
        public string? Pincode { get; set; }
    }
}
