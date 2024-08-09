using LMS2.Models;
using LMS2.Models.ViewModels.Request;
using LMS2.Models.ViewModels.Search;
using LMS2.Repository;
using LMS2.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace LMS2.Controllers
{


    /// <summary>
    /// BorrowRecord Routes
    /// </summary>
    [Route("api/borrowRecords")]
    [ApiController]
    public class BorrowRecordsController : ControllerBase
    {
        private readonly IBorrowRecordsRepository _borrowRecordsRepository;

        /// <summary>
        /// Borrow Record Controller Constructor
        /// </summary>
        public BorrowRecordsController(IBorrowRecordsRepository borrowRecordsRepository)
        {
            if (borrowRecordsRepository != null)
            {
                _borrowRecordsRepository = borrowRecordsRepository;
            }
            else
                throw new ArgumentNullException(nameof(borrowRecordsRepository));
        }
        
        
        /// <summary>
        /// Get All Borrow Records Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var res = _borrowRecordsRepository.GetAllBorrowRecords();
                return new JsonResult(new { data = res });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }
        }
        
        
        /// <summary>
        /// Get Borrow Record Data By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public JsonResult GetByID(int id)
        {
            try
            {
                var res = _borrowRecordsRepository.GetBorrowRecordById(id);
                return new JsonResult(new { data = res });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }
        }
        
        
        /// <summary>
        /// Add New Borrow Record
        /// </summary>
        /// <param name="requestBorrowRecord"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddBorrowRecord(RequestBorrowRecord? requestBorrowRecord)
        {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(requestBorrowRecord);
                _borrowRecordsRepository.AddBorrowRecord(requestBorrowRecord);
                _borrowRecordsRepository.Save();
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }
        }
        
        
        /// <summary>
        /// Delete Existing Borrow Records by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public JsonResult DeleteBorrowRecord(int id)
        {
            try
            {
                _borrowRecordsRepository.DeleteBorrowRecord(id);
                _borrowRecordsRepository.Save();
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }
        }
        
        
        /// <summary>
        /// Patch for updating Borrow Record by BookID, MemberID, borrowDate, dueDate and ReturnDate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="borrowRecord"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public JsonResult PatchMember(int id, RequestBorrowRecord borrowRecord)
        {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(borrowRecord);

                var res = _borrowRecordsRepository.UpdateBorrowRecord(id, borrowRecord);
                _borrowRecordsRepository.Save();
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }
        }



        /// <summary>
        /// Search borrow records by bookId, memberId, borrowDate, dueDate, returnDate, bookName, author, genre, publisherName, memberName, email, mobileNumber, city, pincode
        /// </summary>
        /// <param name="searchBorrow"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("search")]
        public JsonResult GetBorrowRecordBySearch([FromQuery]SearchBorrowRecords searchBorrow, int pageNumber = 1, int pageSize = int.MaxValue)
        {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(searchBorrow);

                ValidationUtility.PageInfoValidator(pageNumber, pageSize);

                var res = _borrowRecordsRepository.GetBorrowRecordsBySearchParams(pageNumber, pageSize, searchBorrow);

                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }

        }
        /// <summary>
        /// Get Overall Penalty
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("penalty/{id}")]
        public JsonResult GetOverAllPenalty(int id)
        {
            try
            {
                var res = _borrowRecordsRepository.GetOverallPenaltyByMemberId(id);
                return new JsonResult(new { message = res });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Notify all users for upcoming return date
        /// </summary>
        /// <returns></returns>
        [HttpGet("notificationForReturn")]
        public JsonResult NotifyAllUsers()
        {
            try
            {
                var borrowRecordsToMail = _borrowRecordsRepository.GetBorrowRecordToNotify();
                foreach (var record in borrowRecordsToMail)
                {
                    try
                    {
                        new EmailSender().SendEmail(record);
                    }
                    catch(Exception ex){
                        return new JsonResult(new { ex.Message , record});
                    }
                }
                return new JsonResult(Ok());
            }
            catch (Exception ex) { 
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }
        }
        /// <summary>
        /// Run Database Integrity Job
        /// </summary>
        /// <returns></returns>
        [HttpGet("databaseIntegrityJob")]
        public JsonResult RunDatabaseIntegrityJob()
        {
            try
            {
                _borrowRecordsRepository.BorrowRecordIntegrity();
                _borrowRecordsRepository.Save();
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message });
            }
        }
    }
}