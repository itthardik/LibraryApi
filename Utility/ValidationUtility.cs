using LMS2.DataContext;
using LMS2.Models;
using LMS2.Models.ViewModels;

namespace LMS2.Utility
{
    /// <summary>
    /// Validation class as utility
    /// </summary>
    static public class ValidationUtility
    {
        /// <summary>
        /// Check gor vaild date of Borrow Records
        /// </summary>
        /// <param name="requestBorrowRecord"></param>
        /// <param name="foundBorrowRecord"></param>
        /// <exception cref="Exception"></exception>
        static public void CheckValidBorrowDate(RequestBorrowRecord requestBorrowRecord, BorrowRecord? foundBorrowRecord = null )
        {
            if (requestBorrowRecord.BorrowDate != null)
            {
                if (requestBorrowRecord.DueDate != null && requestBorrowRecord.BorrowDate > requestBorrowRecord.DueDate) { throw new CustomException(message: "Borrow date can not be after Due Date"); }
                if (requestBorrowRecord.ReturnDate != null && requestBorrowRecord.BorrowDate > requestBorrowRecord.ReturnDate) { throw new CustomException(message: "Borrow date can not be after Return Date"); }
                if (foundBorrowRecord != null)
                {
                    if (requestBorrowRecord.BorrowDate > foundBorrowRecord.DueDate) { throw new CustomException(message: "Borrow date can not be after Due Date"); }
                    if (requestBorrowRecord.BorrowDate > foundBorrowRecord.ReturnDate) { throw new CustomException(message: "Borrow date can not be after Return Date"); }
                }
            }
            else
            {
                if (requestBorrowRecord.DueDate != null && foundBorrowRecord?.BorrowDate > requestBorrowRecord.DueDate) { throw new CustomException(message: "Borrow date can not be after Due Date"); }
                if (requestBorrowRecord.ReturnDate != null && foundBorrowRecord?.BorrowDate > requestBorrowRecord.ReturnDate) { throw new CustomException(message: "Borrow date can not be after Return Date"); }
            }
        }
        /// <summary>
        /// Check Borrow Record Exist Already?
        /// </summary>
        /// <param name="allBorrowRecords"></param>
        /// <param name="requestBorrowRecord"></param>
        /// <exception cref="Exception"></exception>
        static public void IsBorrowRecordAlreadyExist(IQueryable<BorrowRecord> allBorrowRecords, RequestBorrowRecord requestBorrowRecord)
        {
            var check = allBorrowRecords
                        .Where(x => (
                            x.BookId == requestBorrowRecord.BookId &&
                            x.MemberId == requestBorrowRecord.MemberId &&
                            x.BorrowDate == requestBorrowRecord.BorrowDate
                        ))
                        .Any();
            if (check)
                throw new CustomException("Borrow Record with this same BookID & MemberID exist");
        }
        /// <summary>
        /// Check Book Exist Already?
        /// </summary>
        /// <param name="allBooks"></param>
        /// <param name="requestBook"></param>
        /// <exception cref="Exception"></exception>
        static public void IsBookAlreadyExist(IQueryable<Book> allBooks , RequestBook requestBook)
        {
            var check = allBooks.Where(x => x.Title == requestBook.Title)
                        .Any();
            if (check)
                throw new CustomException("Book with this name exist");

        }
        /// <summary>
        /// Check Member Exist Already?
        /// </summary>
        /// <param name="allMembers"></param>
        /// <param name="requestMember"></param>
        /// <exception cref="Exception"></exception>
        static public void IsMemberAlreadyExist(IQueryable<Member> allMembers, RequestMember requestMember) {
            var check = allMembers.Where(x => (
                                    (x.Name == requestMember.Name) &&
                                    (x.Email == requestMember.Email) &&
                                    (x.MobileNumber == requestMember.MobileNumber)
                                    )
                                ).Any();
            if (check)
                throw new CustomException("Member with this name, email and mobile number already exist");
        }
        /// <summary>
        /// Check Object is Null or Empty
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="Exception"></exception>
        static public void ObjectIsNullOrEmpty(Object? obj)
        {
            if (obj == null)
                throw new CustomException("Value cannot be null");

            var flag = false;
            foreach (var property in obj.GetType().GetProperties())
            {
                var propertyValue = property.GetValue(obj);
                if (propertyValue != null)
                {
                    flag = true;
                }
            }

            if (!flag)
                throw new CustomException("Required atleast one field");
        }
        /// <summary>
        /// Validate Page Size and Page Number
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <exception cref="Exception"></exception>
        static public void PageInfoValidator(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                throw new CustomException("Page Number and Page Size cannot be negative");
        }

    }
}
