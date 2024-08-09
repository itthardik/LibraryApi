using LMS2.DataContext;
using LMS2.Models;
using LMS2.Models.ViewModels.Request;
using LMS2.Models.ViewModels.Search;
using LMS2.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace LMS2.Repository
{


    /// <summary>
    /// Member Repo
    /// </summary>
    public class MembersRepository : IMembersRepository
    {
        private readonly ApiContext _context;
        
        
        /// <summary>
        /// Member Repository
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MembersRepository(ApiContext? context)
        {
            if(context != null)
                _context = context;
            else
                throw new ArgumentNullException(nameof(context));
        }
        
        
        /// <summary>
        /// Get All Members
        /// </summary>
        /// <returns></returns>
        private IQueryable<Member> GetAllMembers()
        {
            var allMembers = _context.Members
                                .Where<Member>(m => m.IsDeleted == false)
                                .OrderByDescending(b => b.CreatedAt);
            if (!allMembers.Any())
                throw new CustomException("No Members found");

            return allMembers;
        }
        
        
        /// <summary>
        /// Get Member By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Member GetMemberById(int id)
        {
            var members = GetAllMembers()
                            .Where(b => b.Id == id)
                            .ToList();

            if (members.IsNullOrEmpty())
                throw new CustomException("No member found with this Id");

            return members[0];
        }
        /// <summary>
        /// Get req for all books with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public JsonResult GetAllMemberByPagination(int pageNumber, int pageSize)
        {
            var allMembers = GetAllMembers();

            var maxPages = (int)Math.Ceiling((decimal)(allMembers.Count()) / pageSize);

            var membersByPagination = allMembers.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new JsonResult( new{ maxPages, data = membersByPagination });
        }
        
        /// <summary>
        /// Add new member
        /// </summary>
        /// <param name="requestMember"></param>
        public void AddMember(RequestMember? requestMember)
        {
            if (requestMember == null)
                throw new CustomException("Invalid Format");

            ValidationUtility.IsMemberAlreadyExist(GetAllMembers(), requestMember);

            Member newMember = CustomUtility.ConvertRequestMemberToMember(requestMember);   

            _context.Members.Add(newMember);
        }
        
        
        /// <summary>
        /// Delete Member to Id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMember(int id)
        {
            var foundMember = GetMemberById(id);
            foundMember.IsDeleted = true;
        }


        /// <summary>
        /// Update Book by Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestMember"></param>
        /// <returns></returns>
        public Member UpdateMember(int id, RequestMember requestMember)
        {
            if (id == 0)
                throw new CustomException("Id cannot be Zero");

            if (requestMember == null)
                throw new CustomException("Invalid Format");

            var foundMember = GetMemberById(id);

            CustomUtility.UpdateObject1WithObject2 ( foundMember , requestMember );

            return foundMember;
        }
        
        
        /// <summary>
        /// Search Members
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="newMember"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public JsonResult GetMembersBySearchParams(int pageNumber, int pageSize, SearchMember newMember)
        {
            var result = CustomUtility.FilterMembersBySearchParams ( _context, newMember);

            if (result.IsNullOrEmpty())
            {
                throw new CustomException("No Members Found");
            }
            var maxPages = (int)Math.Ceiling((decimal)(result.Count()) / pageSize);

            var finalData = result.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            return new JsonResult(new {maxPages, data = finalData});
        }


        /// <summary>
        /// Save Changes to DB
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
