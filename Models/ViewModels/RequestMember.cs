using System.ComponentModel.DataAnnotations;

namespace LMS2.Models.ViewModels
{
    /// <summary>
    /// Request Class for Member
    /// </summary>
    public class RequestMember
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
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        /// <summary>
        /// MobileNumber and has regx to validate
        /// </summary>
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number. Phone number must be 10 digits.")]
        public int? MobileNumber { get; set; }
        /// <summary>
        /// Address is not required
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// City is not required
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Pincode and has regx to validate
        /// </summary>
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid pincode. Pincode must be 6 digits.")]
        public string? Pincode { get; set; }
    }
}
