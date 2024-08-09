using LMS2.Models;
using LMS2.Models.ViewModels.Request;
using LMS2.Models.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;

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
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        JsonResult GetAllBooksByPagination(int pageNumber, int pageSize);
        /// <summary>
        /// Get book data by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Book GetBookById(int id);
        /// <summary>
        /// Add new Book 
        /// </summary>
        /// <param name="book"></param>
        void AddBook(RequestBook? book); 
        /// <summary>
        /// Update Book by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        Book UpdateBook(int id, RequestBook book);
        /// <summary>
        /// Delete Book Data
        /// </summary>
        /// <param name="id"></param>
        void DeleteBook(int id);
        /// <summary>
        /// Search Book data
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="book"></param>
        JsonResult GetBooksBySearchParams(int pageNumber, int pageSize, SearchBook book);
        /// <summary>
        /// Save Book data in DB
        /// </summary>
        void Save();
    }
}
