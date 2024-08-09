using System.ComponentModel.DataAnnotations;

namespace LMS2.Models
{
    /// <summary>
    /// Member model
    /// </summary>
    public class Member
    {
        /// <summary>
        /// Primary key with auto increment
        /// </summary>
        [Key]
        public int? Id { get; private set; }
        /// <summary>
        /// Name is required and has limit upto 300
        /// </summary>
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }
        /// <summary>
        /// Email is required and has limit upto 100 and regx to validate
        /// </summary>
        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
        public string? Email { get; set; }
        /// <summary>
        /// MobileNumber is required and has regx to validate
        /// </summary>
        [Required]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Invalid phone number. Phone number must be 10 digits.")]
        public string? MobileNumber { get; set; }
        /// <summary>
        /// Address is not required
        /// </summary>
        public string? Address { get; set; }
        /// <summary>
        /// City is not required
        /// </summary>
        public string? City { get; set; }
        /// <summary>
        /// Pincode is required and has regx to validate
        /// </summary>
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid pincode. Pincode must be 6 digits.")]
        public string? Pincode { get; set; }
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
