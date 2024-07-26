using LMS2.Models;

namespace LMS2.Repository
{
    public interface IBorrowRecordsRepository
    {
        IEnumerable<BorrowRecord> GetAllBorrowRecords();
        BorrowRecord GetBorrowRecordById(int id);
        void AddBorrowRecord(InputBorrowRecord book);
        BorrowRecord UpdateBorrowRecord(int id, InputBorrowRecord inputBorrowRecord);
        BorrowRecord UpdateBorrowRecordsByQuery(int id, int? bookId, int? memberId, DateTime? borrowDate, DateTime? dueDate, DateTime? returnDate);
        void DeleteBorrowRecord(BorrowRecord borrowRecord);
        IEnumerable<BorrowRecord> GetBorrowRecordsBySearchParams(int? bookId, int? memberId, DateTime? borrowDate, DateTime? dueDate, DateTime? returnDate,
                                                                string? bookName, string? author, string? genre, string? publisherName,
                                                                string? memberName, string? email, int? mobileNumber, string? city, string? pincode);
        void Save();
    }
}
