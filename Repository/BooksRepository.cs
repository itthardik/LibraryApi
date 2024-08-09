using LMS2.DataContext;
using LMS2.Models;
using LMS2.Models.ViewModels.Request;
using LMS2.Models.ViewModels.Search;
using LMS2.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text.Json.Serialization;

namespace LMS2.Repository
{


    /// <summary>
    /// Book Repo
    /// </summary>
    public class BooksRepository : IBooksRepository
    {
        private readonly ApiContext _context;
        
        
        /// <summary>
        /// Book Repo Constructor
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BooksRepository(ApiContext? context) { 
            if(context != null)
                _context = context;
            else
                throw new ArgumentNullException(nameof(context));
        }
        /// <summary>
        /// Get All Books
        /// </summary>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        private IQueryable<Book> GetAllBooks()
        {
            var allBooks = _context.Books
                                .Where<Book>(b => b.IsDeleted == false)
                                .OrderByDescending(b => b.CreatedAt);


            if (!allBooks.Any())
                throw new CustomException("No Books found");

            return allBooks;
        }


        /// <summary>
        /// Get All Books with pagination
        /// </summary>
        /// <returns></returns>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        public JsonResult GetAllBooksByPagination(int pageNumber, int pageSize)
        {

            var allBooks = GetAllBooks();

            var maxPages = (int)Math.Ceiling((decimal)(allBooks.Count()) / pageSize);

            var booksByPagination = allBooks.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new JsonResult(new { maxPages, data = booksByPagination });
        }
        
        
        /// <summary>
        /// Get Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Book GetBookById(int id) {
            var books = GetAllBooks()
                            .Where( b => b.Id == id)
                            .ToList();

            if (books.IsNullOrEmpty())
                throw new CustomException("No book found with this Id");

            return books[0];
        }
        
        
        /// <summary>
        /// Add new Book
        /// </summary>
        /// <param name="requestBook"></param>
        /// <exception cref="Exception"></exception>
        public void AddBook(RequestBook? requestBook) {
            
            if (requestBook == null)
                throw new CustomException("Invalid Format");

            ValidationUtility.IsBookAlreadyExist( GetAllBooks(), requestBook);

            Book newBook = CustomUtility.ConvertRequestBookToBook(requestBook);

            _context.Books.Add(newBook);
        }
        
        
        /// <summary>
        /// Delete Book By Id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBook(int id)
        {
            var foundBook = GetBookById(id);
            foundBook.IsDeleted = true;
        }
        
        
        /// <summary>
        /// Update book by id and RequestBook
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Book UpdateBook(int id, RequestBook book)
        {
            if (id == 0)
                throw new CustomException("Id cannot be Zero");

            if(book == null) 
                throw new CustomException("Invalid Format");

            var foundBook = GetBookById(id);

            CustomUtility.UpdateObject1WithObject2(foundBook, book);

            return foundBook;            
        }
        
        
        /// <summary>
        /// Get Filter data by search parms and with pagination
        /// </summary>
        /// <param name="newBook"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetBooksBySearchParams( int pageNumber, int pageSize, SearchBook newBook)
            {

            var result = CustomUtility.FilterBooksBySearchParams( _context, newBook);
            
            if (result.IsNullOrEmpty())
                throw new CustomException("No Books Found");

            var maxPages = (int)Math.Ceiling((decimal)(result.Count()) / pageSize);

            var finalData = result.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return new JsonResult(new { maxPages, data = finalData });
        }
        
        
        /// <summary>
        /// Save Changes to DB
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
