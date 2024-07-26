using LMS2.DataContext;
using LMS2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Net;
using System.Reflection;

namespace LMS2.Repository
{
    public class BorrowRecordsRepository : IBorrowRecordsRepository
    {
        private readonly ApiContext _context;
        public BorrowRecordsRepository(ApiContext context)
        {
            _context = context;
        }

        public IEnumerable<BorrowRecord> GetAllBorrowRecords()
        {
            return _context.borrowRecords
                    .Include(br => br.Member)
                    .Include(br => br.Book)
                    .ToList();
        }
        public BorrowRecord? GetBorrowRecordById(int id)
        {
            return _context.borrowRecords
                            .Include(br => br.Member)
                            .Include(br => br.Book)
                            .ToList()
                            .Find(br => br.Id == id);
        }
        public void AddBorrowRecord(InputBorrowRecord inputBorrowRecord)
        {
            BorrowRecord borrowRecord = new BorrowRecord();
            borrowRecord.BookId = inputBorrowRecord.BookId;
            borrowRecord.MemberId = inputBorrowRecord.MemberId;
            if (inputBorrowRecord.BorrowDate > inputBorrowRecord.DueDate || inputBorrowRecord.BorrowDate > inputBorrowRecord.ReturnDate) {
                throw new Exception(message:"Borrow date can not be after Due or Return Dates");
            }
            borrowRecord.BorrowDate = inputBorrowRecord.BorrowDate;
            borrowRecord.DueDate = inputBorrowRecord.DueDate;
            borrowRecord.ReturnDate = inputBorrowRecord.ReturnDate;

            var book = _context.books.Find(inputBorrowRecord.BookId);
            if (book == null)
            {
                throw new Exception(message: "No Book with this Book Id");
            }
            borrowRecord.Book = book;

            var member = _context.members.Find(inputBorrowRecord.MemberId);
            if (member == null)
            {
                throw new Exception(message: "No Member with this Member Id");
            }
            borrowRecord.Member = member;
            if (book.Current_Stock <= 0)
                throw new Exception(message: "No Stock of Specified Book");
            else
                book.Current_Stock--;
            
            _context.borrowRecords.Add(borrowRecord);
            return;
        }
        public void DeleteBorrowRecord(BorrowRecord borrowRecord)
        {
            var book = _context.books.Find(borrowRecord.BookId);
            _context.borrowRecords.Remove(borrowRecord);
            book.Current_Stock++;
                
        }
        public BorrowRecord UpdateBorrowRecord(int id,InputBorrowRecord inputBorrowRecord)
        {
            try
            {
                var foundBorrowRecord = _context.borrowRecords
                                        .Include(br => br.Member)
                                        .Include(br => br.Book)
                                        .ToList().Find(b => b.Id == id);
                if (foundBorrowRecord != null)
                {
                    if(foundBorrowRecord.BookId != inputBorrowRecord.BookId)
                    {
                        var prevBook = _context.books.Find(foundBorrowRecord.BookId);
                        var newBook = _context.books.Find(inputBorrowRecord.BookId);
                        if (newBook == null) {
                            throw new Exception(message: "No Book with this Book Id");
                        }
                        if (newBook.Current_Stock <=0) {
                            throw new Exception(message: "No stock of specified Book");
                        }
                        foundBorrowRecord.Book = newBook;
                        prevBook.Current_Stock++;
                        newBook.Current_Stock--;
                    }
                    if (foundBorrowRecord.MemberId != inputBorrowRecord.MemberId)
                    {
                        var member = _context.members.Find(inputBorrowRecord.MemberId);
                        if (member == null)
                        {
                            throw new Exception(message: "No Member with this Member Id");
                        }
                        foundBorrowRecord.Member = member;
                    }

                    foundBorrowRecord.BookId = inputBorrowRecord.BookId;
                    foundBorrowRecord.MemberId = inputBorrowRecord.MemberId;

                    if (inputBorrowRecord.BorrowDate > inputBorrowRecord.DueDate || inputBorrowRecord.BorrowDate > inputBorrowRecord.ReturnDate)
                    {
                        throw new Exception(message: "Borrow date can not be after Due or Return Dates");
                    }

                    foundBorrowRecord.BorrowDate = inputBorrowRecord.BorrowDate;
                    foundBorrowRecord.DueDate = inputBorrowRecord.DueDate;
                    foundBorrowRecord.ReturnDate = inputBorrowRecord.ReturnDate;

                    return foundBorrowRecord;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public BorrowRecord UpdateBorrowRecordsByQuery(int id, int? bookId, int? memberId, DateTime? borrowDate, DateTime? dueDate, DateTime? returnDate)
        {
            try
            {
                var foundBorrowRecord = _context.borrowRecords
                                        .Include(br => br.Member)
                                        .Include(br => br.Book)
                                        .ToList().Find(b => b.Id == id);
                if (foundBorrowRecord != null)
                {
                    if (bookId != null && foundBorrowRecord.BookId != bookId)
                    {
                        var prevBook = _context.books.Find(foundBorrowRecord.BookId);
                        var newBook = _context.books.Find(bookId);
                        if (newBook == null)
                        {
                            throw new Exception(message: "No Book with this Book Id");
                        }
                        if (newBook.Current_Stock <= 0)
                        {
                            throw new Exception(message: "No stock of specified Book");
                        }
                        foundBorrowRecord.Book = newBook;
                        prevBook.Current_Stock++;
                        newBook.Current_Stock--;
                    }
                    if (memberId != null && foundBorrowRecord.MemberId != memberId)
                    {
                        var member = _context.members.Find(memberId);
                        if (member == null)
                        {
                            throw new Exception(message: "No Member with this Member Id");
                        }
                        foundBorrowRecord.Member = member;
                    }
                    
                    foundBorrowRecord.BookId = bookId?? foundBorrowRecord.BookId;

                    foundBorrowRecord.MemberId = memberId?? foundBorrowRecord.MemberId;

                    if ((borrowDate??foundBorrowRecord.BorrowDate) > (dueDate?? foundBorrowRecord.DueDate) || (borrowDate?? foundBorrowRecord.BorrowDate) > (returnDate?? foundBorrowRecord.ReturnDate))
                    {
                        throw new Exception(message: "Borrow date can not be after Due or Return Dates");
                    }

                    foundBorrowRecord.BorrowDate = borrowDate?? foundBorrowRecord.BorrowDate;
                    foundBorrowRecord.DueDate = dueDate?? foundBorrowRecord.DueDate;
                    foundBorrowRecord.ReturnDate = returnDate?? foundBorrowRecord.ReturnDate;

                    return foundBorrowRecord;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public IEnumerable<BorrowRecord> GetBorrowRecordsBySearchParams(int? bookId, int? memberId, DateTime? borrowDate, DateTime? dueDate, DateTime? returnDate,
                                                                        string? bookName, string? author, string? genre, string? publisherName, 
                                                                        string? memberName, string? email, int? mobileNumber, string? city, string? pincode )
        {
            var allBorrowRecords = _context.borrowRecords
                                    .Include(br => br.Member)
                                    .Include(br => br.Book)
                                    .ToList();
            var result = allBorrowRecords.FindAll(a =>
                            (bookId != null && a.BookId == bookId) ||
                            (memberId != null && a.MemberId == memberId) ||
                            (borrowDate != null && a.BorrowDate == borrowDate) ||
                            (dueDate != null && a.DueDate == dueDate) ||
                            (returnDate != null && a.ReturnDate == returnDate) ||
                            (bookName != null && a.Book.Title.Contains(bookName)) ||
                            (author != null && a.Book.Author_Name.Contains(author)) ||
                            (genre != null && a.Book.Genre.Contains(genre)) ||
                            (publisherName != null && a.Book.Publiser_Name.Contains(publisherName)) ||
                            (memberName != null && a.Member.Name.Contains(memberName)) ||
                            (email != null && a.Member.Email.Contains(email)) ||
                            (mobileNumber != null && a.Member.MobileNumber.ToString().Contains(mobileNumber.ToString())) ||
                            (city != null && a.Member.City.Contains(city)) ||
                            (pincode != null && a.Member.Pincode.Contains(pincode))
                        );
            return result;
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
