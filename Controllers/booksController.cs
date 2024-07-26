using LMS2.DataContext;
using LMS2.Models;
using LMS2.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace LMS2.Controllers
{
    /// <summary>
    /// Book Routes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class booksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository ;
        public booksController(IBooksRepository booksRepository) {
            _booksRepository = booksRepository;
        }
        /// <summary>
        /// Get All Books Data
        /// </summary>
        [HttpGet]
        public JsonResult Get() {
            var res = _booksRepository.GetAllBooks();
            if (res.IsNullOrEmpty()) {
                return new JsonResult(new { message= "The array is empty" });
            }
            return new JsonResult(new { data = res });
        }
        /// <summary>
        /// Get Book Data By Id
        /// </summary>
        [HttpGet("{id}")]
        public JsonResult GetByID(int id) { 
            var res = _booksRepository.GetBookById(id);
            if (res == null)
            {
                return new JsonResult(new { message = "No book found with this Id" });
            }
            return new JsonResult(new { data = res });
        }
        /// <summary>
        /// Add New Book
        /// </summary>
        [HttpPost]
        public JsonResult AddBook(Book book)
        {
            var check = _booksRepository.GetAllBooks().Where(x=>x.Title == book.Title);
            if (!check.IsNullOrEmpty())
                return new JsonResult(new {message = "Book with this title already existed" });

            try
            {
                _booksRepository.AddBook(book);
            }
            catch (Exception ex) {
                return new JsonResult(new {errorMessage = ex.Message});
            }
            _booksRepository.Save();
            return new JsonResult(Ok());
        }
        /// <summary>
        /// Delete Existing Book by Id
        /// </summary>
        [HttpDelete]
        public JsonResult DeleteBook(int id)
        {
            var book = _booksRepository.GetBookById(id);
            if (book == null)
            {
                return new JsonResult(new { message = "No book found with this Id" });
            }
            _booksRepository.DeleteBook(book);
            _booksRepository.Save();
            return new JsonResult(Ok());
        }
        /// <summary>
        /// Update whole Book Data
        /// </summary>
        [HttpPut("{id}")]
        public JsonResult PutBook(int id,Book book)
        {
            if(id == null)
            {
                return new JsonResult(new { message = "Id parameter is required" });
            }
            var res = _booksRepository.UpdateBook(id,book);
            if(res == null)
            {
                return new JsonResult(new { message = "No Book Found with this id" });
            }
            _booksRepository.Save();
            return new JsonResult(res);
        }
        /// <summary>
        /// Patch for updating book by title, description, genre, author, pub_name, pub_des, price, stock
        /// </summary>
        [HttpPatch("{id}")]
        public JsonResult PatchBook(int id, string? title, string? description, string? genre, string? author, string? pub_name, string? pub_des, int? price, int? stock)
        {
            if (id == null)
            {
                return new JsonResult(new { message = "Id parameter is required" });
            }
            if (title == null && description == null && genre == null && author == null && pub_name == null && price == null && stock == null) {
                return new JsonResult(new { message = "atleast one field is required to patch book" });
            }
            if( title?.Length >= 300 ||
                genre?.Length >= 300 ||
                author?.Length >=300 ||
                pub_name?.Length >=300 ||
                price < 0 ||
                stock < 0
                )
            {
                return new JsonResult(new { message = "Invalid format" });
            }
            var res = _booksRepository.UpdateBookByQuery(id, title, description, genre, author, pub_name, pub_des, price, stock);
            if (res == null)
            {
                return new JsonResult(new { message = "No Book Found with this id" });
            }
            _booksRepository.Save();
            return new JsonResult(res);
        }

        /// <summary>
        /// Search book by Title, Genre, AuthorName, PublicationName
        /// </summary>
        [HttpGet("search")]
        public JsonResult GetBookBySearch(string? title, string? genre, string? authorName, string? publicationName) { 
            if(title == null && genre == null && authorName == null && publicationName == null)
                return new JsonResult(new {message = "provide atleast one params"});

            var res = _booksRepository.GetBooksBySearchParams(title, genre, authorName, publicationName);
            
            if (res.IsNullOrEmpty())
                return new JsonResult(new { message = "No Book Found" });

            return new JsonResult(new { message = res });
        }
    }
}
