using LMS2.DataContext;
using LMS2.Models.ViewModels;
using LMS2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace LMS2.Utility
{
    /// <summary>
    /// Utility Class
    /// </summary>
    static public class CustomUtility
    {
        /// <summary>
        /// Get Book by ID using Context Object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public Book GetBookDataFromIdByContext(ApiContext context, int? id)
        {
            if(id == null)
                throw new CustomException("Invalid Id");

            var allBooks = context.Books.Where<Book>(b => b.IsDeleted == false);

            if (!allBooks.Any())
                throw new CustomException("No Books Data found");

            var book = allBooks.Where(b => b.Id == id);

            if (book.IsNullOrEmpty())
                throw new CustomException("No book found with this Id");

            return book.ToList()[0];
        }
        /// <summary>
        /// Get Member by ID using Context Object
        /// </summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public Member GetMemberDataFromIdByContext(ApiContext context, int? id)
        {
            if (id == null)
                throw new CustomException("Invalid Id");

            var allMembers = context.Members.Where<Member>(b => b.IsDeleted == false);

            if (!allMembers.Any())
                throw new CustomException("No Members Data found");

            var member = allMembers.Where(b => b.Id == id);

            if (member.IsNullOrEmpty())
                throw new CustomException("No member found with this Id");

            return member.ToList()[0];
        }
        /// <summary>
        /// Filter for Search Params
        /// </summary>
        /// <param name="allBorrowRecords"></param>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        static public IQueryable<BorrowRecord> FilterBorrowRecordsBySearchParams(IQueryable<BorrowRecord> allBorrowRecords, SearchBorrowRecords searchParams)
        {
            try
            {
                var query = allBorrowRecords.AsQueryable();
                IQueryable<BorrowRecord> finalQuery = query.Where(record => false);

                if (searchParams.BookId != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.BookId == searchParams.BookId));
                }

                if (searchParams.MemberId != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.MemberId == searchParams.MemberId));
                }

                if (searchParams.BorrowDate != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.BorrowDate == searchParams.BorrowDate));
                }

                if (searchParams.DueDate != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.DueDate == searchParams.DueDate));
                }

                if (searchParams.ReturnDate != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.ReturnDate == searchParams.ReturnDate));
                }

                if (searchParams.BookName != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Book != null && record.Book.Title == searchParams.BookName));
                }

                if (searchParams.Author != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Book != null && record.Book.AuthorName == searchParams.Author));
                }

                if (searchParams.Genre != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Book != null && record.Book.Genre == searchParams.Genre));
                }

                if (searchParams.PublisherName != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Book != null && record.Book.PublisherName == searchParams.PublisherName));
                }

                if (searchParams.MemberName != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Member != null && record.Member.Name == searchParams.MemberName));
                }

                if (searchParams.Email != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Member != null && record.Member.Email == searchParams.Email));
                }

                if (searchParams.MobileNumber != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Member != null && record.Member.MobileNumber == searchParams.MobileNumber));
                }

                if (searchParams.City != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Member != null && record.Member.City == searchParams.City));
                }

                if (searchParams.Pincode != null)
                {
                    finalQuery = finalQuery.Union(query.Where(record => record.Member != null && record.Member.Pincode == searchParams.Pincode));
                }

                return finalQuery.OrderByDescending(b => b.CreatedAt); ;
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        /// <summary>
        /// Convert RequestBorrowRecord to BorrowRecords
        /// </summary>
        /// <param name="requestBorrowRecord"></param>
        /// <param name="book"></param>
        /// <param name="member"></param>
        /// <param name="penaltyRate"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public BorrowRecord ConvertRequestBorrowRecordToBorrowRecord(RequestBorrowRecord requestBorrowRecord, Book book, Member member, int penaltyRate)
        {
            return new()
            {
                BookId = requestBorrowRecord.BookId ?? throw new CustomException("Invalid Syntax, missing Book Id"),
                MemberId = requestBorrowRecord.MemberId ?? throw new CustomException("Invalid Syntax, missing Member Id"),
                BorrowDate = requestBorrowRecord.BorrowDate ?? throw new CustomException("Invalid Syntax, missing Borrow Date"),
                DueDate = requestBorrowRecord.DueDate ?? throw new CustomException("Invalid Syntax, missing Due Date"),
                ReturnDate = requestBorrowRecord.ReturnDate ?? null,
                PenaltyAmount = CalculatePenaltyAmount(requestBorrowRecord.DueDate??DateTime.MinValue, requestBorrowRecord.ReturnDate??DateTime.MinValue, penaltyRate),
                Book = book,
                Member = member
            };
        }
        /// <summary>
        /// Convert RequestBook to Book
        /// </summary>
        /// <param name="requestBook"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public Book ConvertRequestBookToBook(RequestBook requestBook)
        {
            return new()
            {

                Title = requestBook.Title ?? throw new CustomException("Invalid Format, Missing Title"),
                Description = requestBook.Description ?? throw new CustomException("Invalid Format, Missing Description"),
                AuthorName = requestBook.AuthorName ?? throw new CustomException("Invalid Format, Missing Author Name"),
                Genre = requestBook.Genre ?? throw new CustomException("Invalid Format, Missing Genre"),
                PublisherName = requestBook.PublisherName ?? throw new CustomException("Invalid Format, Missing Publiser Name"),
                PublisherDescription = requestBook.PublisherDescription ?? throw new CustomException("Invalid Format, Missing Publisher Description"),
                Price = requestBook.Price ?? throw new CustomException("Invalid Format, Missing Price"),
                CurrentStock = requestBook.CurrentStock ?? throw new CustomException("Invalid Format, Missing Current Stock")
            };
        }
        /// <summary>
        /// Convert RequestMember To Member
        /// </summary>
        /// <param name="requestMember"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public Member ConvertRequestMemberToMember(RequestMember requestMember)
        {
            return new()
            {
                Name = requestMember.Name ?? throw new CustomException("Invalid Format, Missing Name"),
                Email = requestMember.Email ?? throw new CustomException("Invalid Format, Missing Email"),
                MobileNumber = requestMember.MobileNumber ?? throw new CustomException("Invalid Format, Missing Mobile Number"),
                Address = requestMember.Address,
                City = requestMember.City,
                Pincode = requestMember.Pincode
            };
        }
        /// <summary>
        /// Update Object 1 using Object 2
        /// </summary>
        /// <param name="Obj1"></param>
        /// <param name="Obj2"></param>
        static public void UpdateObject1WithObject2(Object Obj1, Object Obj2) {
            var propertyValues = new Dictionary<string, object>();
            foreach (var property in Obj2.GetType().GetProperties())
            {
                var propertyValue = property.GetValue(Obj2);
                if (propertyValue != null)
                {
                    propertyValues.Add(property.Name, propertyValue);
                }
            }

            foreach (var property in Obj1.GetType().GetProperties())
            {
                if (!propertyValues.ContainsKey(property.Name))
                {
                    continue;
                }
                var propertyValue = propertyValues[property.Name];
                var propertyType = property.PropertyType;
                var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;


                if (targetType.IsEnum)
                {
                    propertyValue = Enum.ToObject(targetType, propertyValue);
                }
                else if (propertyValue is IConvertible)
                {
                    propertyValue = Convert.ChangeType(propertyValue, targetType);
                }

                property.SetValue(Obj1, propertyValue);

            }
        }
        /// <summary>
        /// Filter Books By Search Params
        /// </summary>

        static public (IQueryable<Book>, int) FilterBooksBySearchParams(IQueryable<Book> allBooks, RequestBook book, int pageNumber, int pageSize)
        {
            try { 
            var query = allBooks.AsQueryable();

            IQueryable<Book> finalQuery = query.Where(record => false);

            if (book.Title != null)
            {
                finalQuery = finalQuery.Union(query.Where(record => (record.Title??"").Contains(book.Title)));
            }

            if (book.AuthorName != null)
            {
                finalQuery = finalQuery.Union(query.Where(record => (record.AuthorName??"").Contains(book.AuthorName)));
            }

            if (book.Genre != null)
            {
                finalQuery = finalQuery.Union(query.Where(record => (record.Genre??"").Contains(book.Genre)));
            }

            if (book.PublisherName != null)
            {
                finalQuery = finalQuery.Union(query.Where(record => (record.PublisherName??"").Contains(book.PublisherName)));
            }

            var maxPages = (int)Math.Ceiling((decimal )(finalQuery.Count()) / pageSize);

            var finalData = finalQuery.Skip(pageSize * (pageNumber - 1)).Take(pageSize);

            return (finalData, maxPages);
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        
        /// <summary>
        /// Filter Members By Search Params
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        static public IEnumerable<Member> FilterMembersBySearchParams(ApiContext _context, SearchMember member)
        {
            try
            {
                return _context.Members.FromSqlRaw<Member>("EXEC SearchMembersByParam @name, @email, @mobileNumber, @pincode;",
                                                     new SqlParameter("@name", member.Name??(object)DBNull.Value),
                                                     new SqlParameter("@email", member.Email ?? (object)DBNull.Value),
                                                     new SqlParameter("@mobileNumber", member.MobileNumber ?? (object)DBNull.Value),
                                                     new SqlParameter("@pincode", member.Pincode ?? (object)DBNull.Value)
                                                     ).AsEnumerable<Member>();
            }
            catch (Exception ex)
            {
                throw new CustomException(ex.Message);
            }
        }
        /// <summary>
        /// Calculate penalty amount
        /// </summary>
        /// <param name="DueDate"></param>
        /// <param name="ReturnDate"></param>
        /// <param name="penaltyRate"></param>
        /// <returns></returns>
        static public int CalculatePenaltyAmount(DateTime DueDate, DateTime ReturnDate, int penaltyRate)
        {
            if(ReturnDate == DateTime.MinValue || DueDate == DateTime.MinValue)
            {
                return 0;
            }

            if (DateTime.Compare(ReturnDate, DueDate) >0)
            {
                return ReturnDate.Subtract(DueDate).Days * penaltyRate;
            }
            return 0;
        }

    }
}
