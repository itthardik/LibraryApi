using LMS2.Controllers;
using LMS2.DataContext;
using LMS2.Models;
using LMS2.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace LMS2.Controllers
{
    /// <summary>
    /// BorrowRecord Routes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class borrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordsRepository _borrowRecordsRepository;
        public borrowRecordsController(IBorrowRecordsRepository borrowRecordsRepository)
        {
            _borrowRecordsRepository = borrowRecordsRepository;
        }
        /// <summary>
        /// Get All Borrow Records Data
        /// </summary>
        [HttpGet]
        public JsonResult Get()
        {
            var res = _borrowRecordsRepository.GetAllBorrowRecords();
            if (res.IsNullOrEmpty())
            {
                return new JsonResult(new { message = "The array is empty" });
            }
            return new JsonResult(new { data = res });
        }
        /// <summary>
        /// Get Borrow Record Data By Id
        /// </summary>
        [HttpGet("{id}")]
        public JsonResult GetByID(int id)
        {
            var res = _borrowRecordsRepository.GetBorrowRecordById(id);
            if (res == null)
            {
                return new JsonResult(new { message = "No borrow record found with this Id" });
            }
            return new JsonResult(new { data = res });
        }
        /// <summary>
        /// Add New Borrow Record
        /// </summary>
        [HttpPost]
        public JsonResult AddBorrowRecord(InputBorrowRecord inputBorrowRecord)
        {
            var check = _borrowRecordsRepository.GetAllBorrowRecords()
                            .Where(x => (
                                x.BookId == inputBorrowRecord.BookId) &&
                                (x.MemberId == inputBorrowRecord.MemberId) &&
                                (x.BorrowDate == inputBorrowRecord.BorrowDate)
                            );
            if (!check.IsNullOrEmpty())
                return new JsonResult(new { message = "BorrowRecord with this BookID, MemberID and Borrow Date already existed" });

            try
            {
                _borrowRecordsRepository.AddBorrowRecord(inputBorrowRecord);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { errorMessage = ex.Message });
            }
            _borrowRecordsRepository.Save();
            return new JsonResult(Ok());
        }
        /// <summary>
        /// Delete Existing Borrow Records by Id
        /// </summary>
        [HttpDelete]
        public JsonResult DeleteBorrowRecord(int id)
        {
            var borrowRecord = _borrowRecordsRepository.GetBorrowRecordById(id);
            if (borrowRecord == null)
            {
                return new JsonResult(new { message = "No borrowRecord found with this Id" });
            }
            _borrowRecordsRepository.DeleteBorrowRecord(borrowRecord);
            _borrowRecordsRepository.Save();
            return new JsonResult(Ok());
        }
        /// <summary>
        /// Update whole Borrow Records Data
        /// </summary>
        [HttpPut("{id}")]
        public JsonResult PutBorrowRecord(int id, InputBorrowRecord inputBorrowRecord)
        {
            if (id == null)
            {
                return new JsonResult(new { message = "Id parameter is required" });
            }
            var res = _borrowRecordsRepository.UpdateBorrowRecord(id, inputBorrowRecord);
            if (res == null)
            {
                return new JsonResult(new { message = "No BorrowRecord Found with this id" });
            }
            _borrowRecordsRepository.Save();
            return new JsonResult(res);
        }
        /// <summary>
        /// Patch for updating Borrow Record by BookID, MemberID, borrowDate, dueDate and ReturnDate
        /// </summary>
        [HttpPatch("{id}")]
        public JsonResult PatchMember(int id, int? bookId, int? memberId, DateTime? borrowDate, DateTime? dueDate, DateTime? returnDate)
        {
            if (id == null)
            {
                return new JsonResult(new { message = "Id parameter is required" });
            }
            if (bookId == null && memberId == null && borrowDate == null && dueDate == null && returnDate == null)
            {
                return new JsonResult(new { message = "atleast one field is required to patch borrow record" });
            }

            var res = _borrowRecordsRepository.UpdateBorrowRecordsByQuery(id, bookId, memberId, borrowDate, dueDate, returnDate);
            if (res == null)
            {
                return new JsonResult(new { message = "No Member Found with this id" });
            }
            _borrowRecordsRepository.Save();
            return new JsonResult(res);
        }
        /// <summary>
        /// Search borrow records by bookId, memberId, borrowDate, dueDate, returnDate, bookName, author, genre, publisherName, memberName, email, mobileNumber, city, pincode
        /// </summary>
        [HttpGet("search")]
        public JsonResult GetBorrowRecordBySearch(int? bookId, int? memberId, DateTime? borrowDate, DateTime? dueDate, DateTime? returnDate,
                                                                        string? bookName, string? author, string? genre, string? publisherName,
                                                                        string? memberName, string? email, int? mobileNumber, string? city, string? pincode)
        {
            if (bookId == null && memberId == null && borrowDate == null && dueDate == null && returnDate == null && bookName == null && author == null && genre == null && publisherName == null &&memberName == null && email == null && mobileNumber == null && city == null && pincode == null )
                return new JsonResult(new { message = "provide atleast one params" });

            var res = _borrowRecordsRepository.GetBorrowRecordsBySearchParams(bookId, memberId, borrowDate, dueDate, returnDate,
                                                                        bookName, author, genre, publisherName,
                                                                        memberName, email, mobileNumber, city, pincode );

            if (res.IsNullOrEmpty())
                return new JsonResult(new { message = "No BorrowRecord Found" });

            return new JsonResult(new { message = res });
        }
    }
}