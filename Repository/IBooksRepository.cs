using LMS2.Models;
using LMS2.Models.ViewModels;

namespace LMS2.Repository
{
    /// <summary>
    /// Interface of Book Repo
    /// </summary>
    public interface IBooksRepository
    {
        /// <summary>
        /// Get all book data
        /// </summary>
        IQueryable<Book> GetAllBooks();
        /// <summary>
        /// Get book data by ID
        /// </summary>
        Book GetBookById(int id);
        /// <summary>
        /// Add new Book 
        /// </summary>
        void AddBook(InputBook? book); 
        /// <summary>
        /// Update Book by ID
        /// </summary>
        Book UpdateBook(int id, InputBook book);
        /// <summary>
        /// Delete Book Data
        /// </summary>
        void DeleteBook(int id);
        /// <summary>
        /// Search Book data
        /// </summary>
        IQueryable<Book> GetBooksBySearchParams(int pageNumber, int pageSize, InputBook book);
        /// <summary>
        /// Save Book data in DB
        /// </summary>
        void Save();
    }
}
