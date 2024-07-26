using LMS2.DataContext;
using LMS2.Models;

namespace LMS2.Repository
{
    public class BooksRepository : IBooksRepository
    {
        private readonly ApiContext _context;
        public BooksRepository(ApiContext context) { 
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.books.ToList();
        }
        public Book? GetBookById(int id) {
            return _context.books.Find(id);
        }
        public void AddBook(Book book) { 
            _context.books.Add(book);
            return;
        }
        public void DeleteBook(Book book)
        {
            _context.books.Remove(book);
            return;
        }
        public Book UpdateBook(int id, Book book) {
            try
            {
                var foundBook = _context.books.ToList().Find(b => b.Id == id);
                if (foundBook != null)
                {
                    foundBook.Title = book.Title;
                    foundBook.Description = book.Description;
                    foundBook.Genre = book.Genre;
                    foundBook.Author_Name = book.Author_Name;
                    foundBook.Price= book.Price;
                    foundBook.Current_Stock = book.Current_Stock;
                    foundBook.Publiser_Name = book.Publiser_Name;
                    foundBook.Publiser_Description = book.Publiser_Description;

                    return foundBook;
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public Book UpdateBookByQuery(int id, string? title, string? description, string? genre, string? author, string? pub_name, string? pub_des, int? price, int? stock)
        {
            try
            {
                var foundBook = _context.books.ToList().Find(b => b.Id == id);
                if (foundBook != null)
                {
                    foundBook.Title = title??foundBook.Title;
                    foundBook.Description =description?? foundBook.Description;
                    foundBook.Genre =genre??foundBook.Genre ; 
                    foundBook.Author_Name =author??foundBook.Author_Name ; 
                    foundBook.Price =price??foundBook.Price ; 
                    foundBook.Current_Stock =stock??foundBook.Current_Stock ; 
                    foundBook.Publiser_Name =pub_name??foundBook.Publiser_Name ; 
                    foundBook.Publiser_Description =pub_des??foundBook.Publiser_Description ; 

                    return foundBook;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public IEnumerable<Book> GetBooksBySearchParams(string? title, string? genre, string? authorName, string? publicationName)
        {
            var allBooks = _context.books.ToList();
            var result = allBooks.FindAll(a=> 
                            (title!=null && a.Title.Contains(title)) ||
                            (genre != null && a.Genre.Contains(genre)) ||
                            (authorName != null && a.Author_Name.Contains(authorName)) ||
                            (publicationName != null && a.Publiser_Name.Contains(publicationName))
                        );
            return result;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
