using Microsoft.EntityFrameworkCore;

namespace LMS2.DataContext
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        public DbSet<LMS2.Models.Book> books { get; set; }
        public DbSet<LMS2.Models.Member> members { get; set; }
        public DbSet<LMS2.Models.BorrowRecord> borrowRecords { get; set; }
    }
}
