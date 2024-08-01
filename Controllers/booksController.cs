using LMS2.Models.ViewModels;
using LMS2.Repository;
using LMS2.Utility;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace LMS2.Controllers
{
    
    
    /// <summary>
    /// Book Routes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBooksRepository _booksRepository ;
        
        
        /// <summary>
        /// Book Controller
        /// </summary>
        /// <param name="booksRepository"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BooksController(IBooksRepository booksRepository) {
            if(booksRepository != null)
                _booksRepository = booksRepository;
            else
                throw new ArgumentNullException(nameof(booksRepository));
        }
        
        
        /// <summary>
        /// Get All Books Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get() {
            try
            {
                var res = _booksRepository.GetAllBooks();
               
                return new JsonResult(new { data = res });
            }
            catch (Exception ex) { 
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
        
        
        /// <summary>
        /// Get Book Data By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public JsonResult GetByID(int id) {
            try
            {
                var res = _booksRepository.GetBookById(id);
                return new JsonResult(new { data = res });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
        
        
        /// <summary>
        /// Add New Book
        /// </summary>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddBook(InputBook? book)
        {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(book);
                _booksRepository.AddBook(book);
                _booksRepository.Save();
                return new JsonResult(Ok());
            }
            catch (Exception ex) {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
        
        
        /// <summary>
        /// Delete Existing Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public JsonResult DeleteBook(int id)
        {
            try
            {
                _booksRepository.DeleteBook(id);
                _booksRepository.Save();
                return new JsonResult(Ok());
            }
            catch (Exception ex) {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
        
        
        /// <summary>
        /// Patch for updating book by title, description, genre, author, pub_name, pub_des, price, stock
        /// </summary>
        /// <param name="id"></param>
        /// <param name="book"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public JsonResult PatchBook(int id, InputBook book)
        {           
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(book);

                var res = _booksRepository.UpdateBook(id, book);
                _booksRepository.Save();
                return new JsonResult(res);
            }
            catch (Exception ex) {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }

        
        
        /// <summary>
        /// Search book by Title, Genre, AuthorName, PublicationName
        /// </summary>
        /// <param name="inputBook"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public JsonResult GetBookBySearch([FromQuery]InputBook inputBook, int pageNumber = 1, int pageSize = int.MaxValue) {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(inputBook);

                ValidationUtility.PageInfoValidator(pageNumber, pageSize);

                var res = _booksRepository.GetBooksBySearchParams( pageNumber, pageSize, inputBook);
                _booksRepository.Save();
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
    }
}
