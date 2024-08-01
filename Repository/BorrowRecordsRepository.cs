using LMS2.DataContext;
using LMS2.Models;
using LMS2.Models.ViewModels;
using LMS2.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LMS2.Repository
{
    
    
    /// <summary>
    /// Borrow Record Repo
    /// </summary>
    public class BorrowRecordsRepository : IBorrowRecordsRepository
    {
        private readonly ApiContext _context;
        
        
        
        /// <summary>
        /// Borrow Record Constructor
        /// </summary>
        /// <param name="context"></param>
        public BorrowRecordsRepository(ApiContext? context)
        {
            if (context != null)
                _context = context;
            else
                throw new ArgumentNullException(nameof(context));
        }
        
        
        
        /// <summary>
        /// Get all Borrow Records
        /// </summary>
        /// <returns></returns>
        public IQueryable<BorrowRecord> GetAllBorrowRecords()
        {
            var allBooks = _context.BorrowRecords
                                    .Where<BorrowRecord>(b => b.IsDeleted == false)
                                    .Include(br => br.Member)
                                    .Include(br => br.Book);
            if (!allBooks.Any())
                throw new Exception("No Books found");

            return allBooks;
        }
        
        
        
   /// <summary>
        /// Get Borrow Record By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public BorrowRecord GetBorrowRecordById(int id)
        {
            var books = _context.BorrowRecords
                            .Where(br => br.Id == id)
                            .Where<BorrowRecord>(b => b.IsDeleted == false)
                            .Include(br => br.Member)
                            .Include(br => br.Book)
                            .ToList();
            if (books.IsNullOrEmpty())
                throw new Exception("No Borrow Record found with this Id");

            return books[0];
        }
        
        
        
        /// <summary>
        /// Add new Borrow Record
        /// </summary>
        /// <param name="inputBorrowRecord"></param>
        /// <exception cref="Exception"></exception>
        public void AddBorrowRecord(InputBorrowRecord? inputBorrowRecord)
        {

            if (inputBorrowRecord == null)
                throw new Exception("Invalid Format");

            ValidationUtility.IsBorrowRecordAlreadyExist(GetAllBorrowRecords(), inputBorrowRecord);

            ValidationUtility.CheckValidBorrowDate(inputBorrowRecord);

            var book = CustomUtility.GetBookDataFromIdByContext(_context, inputBorrowRecord.BookId);

            var member = CustomUtility.GetMemberDataFromIdByContext(_context, inputBorrowRecord.MemberId);

            if (book.CurrentStock <= 0)
                throw new Exception(message: "No Stock of Specified Book");
            else
                book.CurrentStock--;


            BorrowRecord newBorrowRecord = CustomUtility.ConvertInputBorrowRecordToBorrowRecord(inputBorrowRecord, book, member);

            _context.BorrowRecords.Add(newBorrowRecord);
        }
        
        
        
        /// <summary>
        /// Delete Borrow Record By ID
        /// </summary>
        /// <param name="id"></param>
        public void DeleteBorrowRecord(int id)
        {
            var foundBorrowRecord = GetBorrowRecordById(id);
            foundBorrowRecord.IsDeleted = true;

            var book = _context.Books.Where( i => i.Id == foundBorrowRecord.BookId).ToList()[0];
            book.CurrentStock++;
                
        }
        
        
        
        /// <summary>
        /// Update BorrowRecord with ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inputBorrowRecord"></param>
        /// <returns></returns>
        //Todo not working for no books and members
        public BorrowRecord UpdateBorrowRecord(int id,InputBorrowRecord inputBorrowRecord)
        {
            if (id == 0)
                throw new Exception("Id cannot be Zero");
    
            if (inputBorrowRecord == null)
                throw new Exception("Invalid Format");

            var foundBorrowRecord = GetBorrowRecordById(id);


            if ( inputBorrowRecord.BookId != null && foundBorrowRecord.BookId != inputBorrowRecord.BookId)
            {

                var newBook = CustomUtility.GetBookDataFromIdByContext(_context, inputBorrowRecord.BookId);

                var prevBook = CustomUtility.GetBookDataFromIdByContext(_context, foundBorrowRecord.BookId);


                if (newBook.CurrentStock <= 0)
                    throw new Exception(message: "No stock of specified Book");

                prevBook.CurrentStock++;
                newBook.CurrentStock--;

                foundBorrowRecord.Book = newBook;
                foundBorrowRecord.BookId = inputBorrowRecord.BookId ?? foundBorrowRecord.BookId ;
            }

            if (inputBorrowRecord.MemberId != null && foundBorrowRecord.MemberId != inputBorrowRecord.MemberId)
            {
                var member = CustomUtility.GetMemberDataFromIdByContext(_context, inputBorrowRecord.MemberId);

                foundBorrowRecord.Member = member;
                foundBorrowRecord.MemberId = inputBorrowRecord.MemberId ?? foundBorrowRecord.MemberId;
            }

            ValidationUtility.CheckValidBorrowDate(inputBorrowRecord, foundBorrowRecord);

            foundBorrowRecord.BorrowDate = inputBorrowRecord.BorrowDate ?? foundBorrowRecord.BorrowDate;
            foundBorrowRecord.DueDate = inputBorrowRecord.DueDate ?? foundBorrowRecord.DueDate;
            foundBorrowRecord.ReturnDate = inputBorrowRecord.ReturnDate ?? foundBorrowRecord.ReturnDate;

            foundBorrowRecord.PenaltyAmount = CustomUtility.CalculatePenaltyAmount(foundBorrowRecord.DueDate, foundBorrowRecord.ReturnDate??DateTime.MinValue);

            return foundBorrowRecord;
            
        }
        
        
        /// <summary>
        /// Search Borrow Record by Params
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchBorrowRecord"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public IQueryable<BorrowRecord> GetBorrowRecordsBySearchParams(int pageNumber, int pageSize, SearchBorrowRecords searchBorrowRecord)
        {
            var result = CustomUtility.FilterBorrowRecordsBySearchParams(GetAllBorrowRecords(), searchBorrowRecord)
                            .Skip<BorrowRecord>((pageNumber - 1) * pageSize)
                            .Take<BorrowRecord>(pageSize);

            if (result.IsNullOrEmpty())
                throw new Exception("No Borrow Record Found");

            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public int GetOverallPenaltyByMemberId(int memberId)
        {
            var allBorrowRecordsByMemberId = GetAllBorrowRecords()
                                    .Where(b => b.MemberId == memberId)
                                    .ToList();

            if (allBorrowRecordsByMemberId.IsNullOrEmpty())
                throw new Exception("No member found with this Id");

            var sum = 0;
            foreach(var  b in allBorrowRecordsByMemberId)
            {
                sum += b.PenaltyAmount;
            }

            return sum;
        }



        /// <summary>
        /// Save Changes in DB
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
