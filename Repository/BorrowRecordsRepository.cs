using LMS2.DataContext;
using LMS2.Models;
using LMS2.Models.ViewModels.Request;
using LMS2.Models.ViewModels.Search;
using LMS2.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace LMS2.Repository
{


    /// <summary>
    /// Borrow Record Repo
    /// </summary>
    public class BorrowRecordsRepository : IBorrowRecordsRepository
    {
        private readonly ApiContext _context;
        private readonly int _penaltyRate;
        private readonly int _notificationInterval;
        private readonly int _oldRecordCriteriaInMonths;


        /// <summary>
        /// Borrow Record Constructor
        /// </summary>
        /// <param name="context"></param>
        public BorrowRecordsRepository(ApiContext? context)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _penaltyRate = configuration.GetValue<int>("AppConstraint:PenaltyRate");
            _notificationInterval = configuration.GetValue<int>("AppConstraint:NotificationInterval");
            _oldRecordCriteriaInMonths = configuration.GetValue<int>("AppConstraint:OldRecordCriteriaInMonths");

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
                                    .Include(br => br.Book)
                                    .OrderByDescending(b => b.CreatedAt);
            if (!allBooks.Any())
                throw new CustomException("No Books found");

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
                throw new CustomException("No Borrow Record found with this Id");

            return books[0];
        }
        
        
        
        /// <summary>
        /// Add new Borrow Record
        /// </summary>
        /// <param name="requestBorrowRecord"></param>
        /// <exception cref="Exception"></exception>
        public void AddBorrowRecord(RequestBorrowRecord? requestBorrowRecord)
        {

            if (requestBorrowRecord == null)
                throw new CustomException("Invalid Format");

            var allBorrowRecord = _context.BorrowRecords.Where<BorrowRecord>(b => b.IsDeleted == false);

            ValidationUtility.IsBorrowRecordAlreadyExist(allBorrowRecord, requestBorrowRecord);

            ValidationUtility.CheckValidBorrowDate(requestBorrowRecord);

            var book = CustomUtility.GetBookDataFromIdByContext(_context, requestBorrowRecord.BookId);

            var member = CustomUtility.GetMemberDataFromIdByContext(_context, requestBorrowRecord.MemberId);

            if (book.CurrentStock <= 0)
                throw new CustomException(message: "No Stock of Specified Book");
            else
                book.CurrentStock--;


            BorrowRecord newBorrowRecord = CustomUtility.ConvertRequestBorrowRecordToBorrowRecord(requestBorrowRecord, book, member, _penaltyRate);

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
        /// <param name="requestBorrowRecord"></param>
        /// <returns></returns>
        //Todo not working for no books and members
        public BorrowRecord UpdateBorrowRecord(int id,RequestBorrowRecord requestBorrowRecord)
        {
            if (id == 0)
                throw new CustomException("Id cannot be Zero");
    
            if (requestBorrowRecord == null)
                throw new CustomException("Invalid Format");

            var foundBorrowRecord = GetBorrowRecordById(id);


            if ( requestBorrowRecord.BookId != null && foundBorrowRecord.BookId != requestBorrowRecord.BookId)
            {

                var newBook = CustomUtility.GetBookDataFromIdByContext(_context, requestBorrowRecord.BookId);

                var prevBook = CustomUtility.GetBookDataFromIdByContext(_context, foundBorrowRecord.BookId);


                if (newBook.CurrentStock <= 0)
                    throw new CustomException(message: "No stock of specified Book");

                prevBook.CurrentStock++;
                newBook.CurrentStock--;

                foundBorrowRecord.Book = newBook;
                foundBorrowRecord.BookId = requestBorrowRecord.BookId ?? foundBorrowRecord.BookId ;
            }

            if (requestBorrowRecord.MemberId != null && foundBorrowRecord.MemberId != requestBorrowRecord.MemberId)
            {
                var member = CustomUtility.GetMemberDataFromIdByContext(_context, requestBorrowRecord.MemberId);

                foundBorrowRecord.Member = member;
                foundBorrowRecord.MemberId = requestBorrowRecord.MemberId ?? foundBorrowRecord.MemberId;
            }

            ValidationUtility.CheckValidBorrowDate(requestBorrowRecord, foundBorrowRecord);

            foundBorrowRecord.BorrowDate = requestBorrowRecord.BorrowDate ?? foundBorrowRecord.BorrowDate;
            foundBorrowRecord.DueDate = requestBorrowRecord.DueDate ?? foundBorrowRecord.DueDate;
            foundBorrowRecord.ReturnDate = requestBorrowRecord.ReturnDate ?? foundBorrowRecord.ReturnDate;

            foundBorrowRecord.PenaltyAmount = CustomUtility.CalculatePenaltyAmount(foundBorrowRecord.DueDate, foundBorrowRecord.ReturnDate??DateTime.MinValue, _penaltyRate);

            foundBorrowRecord.IsPenaltyPaid = requestBorrowRecord.IsPenaltyPaid?? foundBorrowRecord.IsPenaltyPaid;

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
                throw new CustomException("No Borrow Record Found");

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
            var allBorrowRecordsByMemberId = _context.BorrowRecords
                                    .Where<BorrowRecord>(b => b.IsDeleted == false)
                                    .Where(b => b.MemberId == memberId)
                                    .Include(br => br.Member)
                                    .Include(br => br.Book)
                                    .OrderByDescending(b => b.CreatedAt)
                                    .ToList();

            if (allBorrowRecordsByMemberId.IsNullOrEmpty())
                throw new CustomException("No Borrow Records found with this Member Id");

            var sum = 0;
            foreach(var  b in allBorrowRecordsByMemberId)
            {
                sum += b.PenaltyAmount;
            }

            return sum;
        }



        /// <summary>
        /// Get Borrow Record to notify
        /// </summary>
        /// <returns></returns>
        public IQueryable<BorrowRecord> GetBorrowRecordToNotify()
        {
            var allBorrowRecord = GetAllBorrowRecords();

            var dayAfterNotificationEnable = DateTime.Now.AddDays(_notificationInterval);
            var currentDate = DateTime.Now;

            var recordsWithReturnDates = allBorrowRecord.Where(b => (b.DueDate >= currentDate && b.DueDate <= dayAfterNotificationEnable) && (b.ReturnDate == null));

            return recordsWithReturnDates;
        }
        /// <summary>
        /// Borrow Record Integrity 
        /// </summary>
        public void BorrowRecordIntegrity()
        {
            var allBorrowRecords = GetAllBorrowRecords();

            var recordsViolatingDatabaseIntegrity = allBorrowRecords.Where(br => (br.Member != null && br.Member.IsDeleted == true) || (br.Book != null && br.Book.IsDeleted == true) );
            foreach (var record in recordsViolatingDatabaseIntegrity)
            {
                record.IsDeleted = true;
            }

            var recordsWhichAreOlder = allBorrowRecords.Where(br => (br.IsPenaltyPaid == true) && ( DateTime.Compare( br.ReturnDate ??DateTime.MaxValue , DateTime.Now.AddMonths(-_oldRecordCriteriaInMonths) ) < 0));
            foreach (var record in recordsWhichAreOlder)
            {
                record.IsDeleted = true;
            }
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
