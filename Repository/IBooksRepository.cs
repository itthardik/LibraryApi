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
        /// Get all book data by pageNumber and pageSize
        /// </summary>
        /// <returns></returns>
        (IQueryable<Book>, int) GetAllBooksByPagination(int pageNumber, int pageSize);
        /// <summary>
        /// Get book data by ID
        /// </summary>
        Book GetBookById(int id);
        /// <summary>
        /// Add new Book 
        /// </summary>
        void AddBook(RequestBook? book); 
        /// <summary>
        /// Update Book by ID
        /// </summary>
        Book UpdateBook(int id, RequestBook book);
        /// <summary>
        /// Delete Book Data
        /// </summary>
        void DeleteBook(int id);
        /// <summary>
        /// Search Book data
        /// </summary>
        (IQueryable<Book>, int) GetBooksBySearchParams(int pageNumber, int pageSize, RequestBook book);
        /// <summary>
        /// Save Book data in DB
        /// </summary>
        void Save();
    }
}
