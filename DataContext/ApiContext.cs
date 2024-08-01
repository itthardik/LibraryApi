using Microsoft.EntityFrameworkCore;

namespace LMS2.DataContext
{
    /// <summary>
    /// Api Context Class
    /// </summary>
    /// <remarks>
    /// Api Context Constructor
    /// </remarks>
    /// <param name="options"></param>
    public class ApiContext(DbContextOptions<ApiContext> options) : DbContext(options)
    {

        /// <summary>
        /// Book DBset
        /// </summary>
        public DbSet<LMS2.Models.Book> Books { get; set; }
        /// <summary>
        /// Member DBset
        /// </summary>
        public DbSet<LMS2.Models.Member> Members { get; set; }
        /// <summary>
        /// Borrow Record DBset
        /// </summary>
        public DbSet<LMS2.Models.BorrowRecord> BorrowRecords { get; set; }
    }
}
