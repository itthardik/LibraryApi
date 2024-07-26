using LMS2.Models;

namespace LMS2.Repository
{
    public interface IBooksRepository
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int id);
        void AddBook(Book book); 
        //TODO
        Book UpdateBook(int id, Book book);

        Book UpdateBookByQuery(int id, string? title, string? description, string? genre, string? author, string? pub_name, string? pub_des, int? price, int? stock);
        void DeleteBook(Book book);
        IEnumerable<Book> GetBooksBySearchParams(string? title, string? genre, string? authorName, string? publicationName);
        void Save();
    }
}
