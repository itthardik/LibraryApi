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
        /// <param name="inputBorrowRecord"></param>
        /// <param name="foundBorrowRecord"></param>
        /// <exception cref="Exception"></exception>
        static public void CheckValidBorrowDate(InputBorrowRecord inputBorrowRecord, BorrowRecord? foundBorrowRecord = null )
        {
            if (inputBorrowRecord.BorrowDate != null)
            {
                if (inputBorrowRecord.DueDate != null && inputBorrowRecord.BorrowDate > inputBorrowRecord.DueDate) { throw new Exception(message: "Borrow date can not be after Due Date"); }
                if (inputBorrowRecord.ReturnDate != null && inputBorrowRecord.BorrowDate > inputBorrowRecord.ReturnDate) { throw new Exception(message: "Borrow date can not be after Return Date"); }
                if (foundBorrowRecord != null)
                {
                    if (inputBorrowRecord.BorrowDate > foundBorrowRecord.DueDate) { throw new Exception(message: "Borrow date can not be after Due Date"); }
                    if (inputBorrowRecord.BorrowDate > foundBorrowRecord.ReturnDate) { throw new Exception(message: "Borrow date can not be after Return Date"); }
                }
            }
            else
            {
                if (inputBorrowRecord.DueDate != null && foundBorrowRecord?.BorrowDate > inputBorrowRecord.DueDate) { throw new Exception(message: "Borrow date can not be after Due Date"); }
                if (inputBorrowRecord.ReturnDate != null && foundBorrowRecord?.BorrowDate > inputBorrowRecord.ReturnDate) { throw new Exception(message: "Borrow date can not be after Return Date"); }
            }
        }
        /// <summary>
        /// Check Borrow Record Exist Already?
        /// </summary>
        /// <param name="allBorrowRecords"></param>
        /// <param name="inputBorrowRecord"></param>
        /// <exception cref="Exception"></exception>
        static public void IsBorrowRecordAlreadyExist(IQueryable<BorrowRecord> allBorrowRecords, InputBorrowRecord inputBorrowRecord)
        {
            var check = allBorrowRecords
                        .Where(x => (
                            x.BookId == inputBorrowRecord.BookId &&
                            x.MemberId == inputBorrowRecord.MemberId &&
                            x.BorrowDate == inputBorrowRecord.BorrowDate
                        ))
                        .Any();
            if (check)
                throw new Exception("Borrow Record with this same BookID & MemberID exist");
        }
        /// <summary>
        /// Check Book Exist Already?
        /// </summary>
        /// <param name="allBooks"></param>
        /// <param name="inputBook"></param>
        /// <exception cref="Exception"></exception>
        static public void IsBookAlreadyExist(IQueryable<Book> allBooks , InputBook inputBook)
        {
            var check = allBooks.Where(x => x.Title == inputBook.Title)
                        .Any();
            if (check)
                throw new Exception("Book with this name exist");

        }
        /// <summary>
        /// Check Member Exist Already?
        /// </summary>
        /// <param name="allMembers"></param>
        /// <param name="inputMember"></param>
        /// <exception cref="Exception"></exception>
        static public void IsMemberAlreadyExist(IQueryable<Member> allMembers, InputMember inputMember) {
            var check = allMembers.Where(x => (
                                    (x.Name == inputMember.Name) &&
                                    (x.Email == inputMember.Email) &&
                                    (x.MobileNumber == inputMember.MobileNumber)
                                    )
                                ).Any();
            if (check)
                throw new Exception("Member with this name, email and mobile number already exist");
        }
        /// <summary>
        /// Check Object is Null or Empty
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="Exception"></exception>
        static public void ObjectIsNullOrEmpty(Object? obj)
        {
            if (obj == null)
                throw new Exception("Value cannot be null");

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
                throw new Exception("Required atleast one field");
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
                throw new Exception("Page Number and Page Size cannot be negative");
        }

    }
}
