using LMS2.DataContext;
using LMS2.Models.ViewModels;
using LMS2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
                throw new Exception("Invalid Id");

            var allBooks = context.Books.Where<Book>(b => b.IsDeleted == false);

            if (!allBooks.Any())
                throw new Exception("No Books Data found");

            var book = allBooks.Where(b => b.Id == id);

            if (book.IsNullOrEmpty())
                throw new Exception("No book found with this Id");

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
                throw new Exception("Invalid Id");

            var allMembers = context.Members.Where<Member>(b => b.IsDeleted == false);

            if (!allMembers.Any())
                throw new Exception("No Members Data found");

            var member = allMembers.Where(b => b.Id == id);

            if (member.IsNullOrEmpty())
                throw new Exception("No member found with this Id");

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

                return finalQuery;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Convert InputBorrowRecord to BorrowRecords
        /// </summary>
        /// <param name="inputBorrowRecord"></param>
        /// <param name="book"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public BorrowRecord ConvertInputBorrowRecordToBorrowRecord(InputBorrowRecord inputBorrowRecord, Book book, Member member)
        {
            return new()
            {
                BookId = inputBorrowRecord.BookId ?? throw new Exception("Invalid Syntax, missing Book Id"),
                MemberId = inputBorrowRecord.MemberId ?? throw new Exception("Invalid Syntax, missing Member Id"),
                BorrowDate = inputBorrowRecord.BorrowDate ?? throw new Exception("Invalid Syntax, missing Borrow Date"),
                DueDate = inputBorrowRecord.DueDate ?? throw new Exception("Invalid Syntax, missing Due Date"),
                ReturnDate = inputBorrowRecord.ReturnDate ?? null,
                PenaltyAmount = CalculatePenaltyAmount(inputBorrowRecord.DueDate??DateTime.MinValue, inputBorrowRecord.ReturnDate??DateTime.MinValue),
                Book = book,
                Member = member
            };
        }
        /// <summary>
        /// Convert InputBook to Book
        /// </summary>
        /// <param name="inputBook"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public Book ConvertInputBookToBook(InputBook inputBook)
        {
            return new()
            {

                Title = inputBook.Title ?? throw new Exception("Invalid Format, Missing Title"),
                Description = inputBook.Description ?? throw new Exception("Invalid Format, Missing Description"),
                AuthorName = inputBook.AuthorName ?? throw new Exception("Invalid Format, Missing Author Name"),
                Genre = inputBook.Genre ?? throw new Exception("Invalid Format, Missing Genre"),
                PublisherName = inputBook.PublisherName ?? throw new Exception("Invalid Format, Missing Publiser Name"),
                PubliserDescription = inputBook.PubliserDescription ?? throw new Exception("Invalid Format, Missing Publiser Description"),
                Price = inputBook.Price ?? throw new Exception("Invalid Format, Missing Price"),
                CurrentStock = inputBook.CurrentStock ?? throw new Exception("Invalid Format, Missing Current Stock")
            };
        }
        /// <summary>
        /// Convert InputMember To Member
        /// </summary>
        /// <param name="inputMember"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        static public Member ConvertInputMemberToMember(InputMember inputMember)
        {
            return new()
            {
                Name = inputMember.Name ?? throw new Exception("Invalid Format, Missing Name"),
                Email = inputMember.Email ?? throw new Exception("Invalid Format, Missing Email"),
                MobileNumber = inputMember.MobileNumber ?? throw new Exception("Invalid Format, Missing Mobile Number"),
                Address = inputMember.Address,
                City = inputMember.City,
                Pincode = inputMember.Pincode
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
       
        static public IQueryable<Book> FilterBooksBySearchParams(ApiContext _context, InputBook book, int pageNumber, int pageSize)
        {
            var whereConditonsString = "";
            foreach (var property in book.GetType().GetProperties())
            {
                var propertyValue = property.GetValue(book);
                if (propertyValue != null)
                {
                    if (whereConditonsString != "")
                        whereConditonsString += " OR ";
                    whereConditonsString += (property.Name+" LIKE '%"+propertyValue+"%'");
                }
            }

            return _context.Books.FromSqlRaw("EXEC SearchTableByParams @tableName, @whereCondition, @pageNumber, @pageSize",
                                                    new SqlParameter("@tableName", "Books"),
                                                    new SqlParameter("@whereCondition", whereConditonsString),
                                                    new SqlParameter("@pageNumber", pageNumber),
                                                    new SqlParameter("@pageSize", pageSize)
                                                    );
        }
        
        /// <summary>
        /// Filter Members By Search Params
        /// </summary>
        /// <param name="_context"></param>
        /// <param name="member"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        static public IQueryable<Member> FilterMembersBySearchParams(ApiContext _context, InputMember member, int pageNumber, int pageSize)
        {
            var whereConditonsString = "";
            foreach (var property in member.GetType().GetProperties())
            {
                var propertyValue = property.GetValue(member);
                if (propertyValue != null)
                {
                    if (whereConditonsString != "")
                        whereConditonsString += " OR ";
                    whereConditonsString += (property.Name + " LIKE '%" + propertyValue + "%'");
                }
            }

            return _context.Members.FromSqlRaw("EXEC SearchTableByParams @tableName, @whereCondition, @pageNumber, @pageSize",
                                                    new SqlParameter("@tableName", "Members"),
                                                    new SqlParameter("@whereCondition", whereConditonsString),
                                                    new SqlParameter("@pageNumber", pageNumber),
                                                    new SqlParameter("@pageSize", pageSize)
                                                    );
        }
        /// <summary>
        /// Calculate penalty amount
        /// </summary>
        /// <param name="DueDate"></param>
        /// <param name="ReturnDate"></param>
        /// <returns></returns>
        static public int CalculatePenaltyAmount(DateTime DueDate, DateTime ReturnDate)
        {
            if(ReturnDate == DateTime.MinValue || DueDate == DateTime.MinValue)
            {
                return 0;
            }

            if (DateTime.Compare(ReturnDate, DueDate) >0)
            {
                return ReturnDate.Subtract(DueDate).Days * 5;
            }
            return 0;
        }

    }
}
