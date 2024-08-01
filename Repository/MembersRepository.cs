using LMS2.DataContext;
using LMS2.Models;
using LMS2.Models.ViewModels;
using LMS2.Utility;
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
        public IQueryable<Member> GetAllMembers()
        {
            var allMembers = _context.Members
                                .Where<Member>(m => m.IsDeleted == false);
            if (!allMembers.Any())
                throw new Exception("No Members found");

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
                throw new Exception("No member found with this Id");

            return members[0];
        }
        
        
        /// <summary>
        /// Add new member
        /// </summary>
        /// <param name="inputMember"></param>
        public void AddMember(InputMember? inputMember)
        {
            if (inputMember == null)
                throw new Exception("Invalid Format");

            ValidationUtility.IsMemberAlreadyExist(GetAllMembers(), inputMember);

            Member newMember = CustomUtility.ConvertInputMemberToMember(inputMember);   

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
        /// <param name="inputMember"></param>
        /// <returns></returns>
        public Member UpdateMember(int id, InputMember? inputMember)
        {
            if (id == 0)
                throw new Exception("Id cannot be Zero");

            if (inputMember == null)
                throw new Exception("Invalid Format");

            var foundMember = GetMemberById(id);

            CustomUtility.UpdateObject1WithObject2 ( foundMember , inputMember );

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
        public IQueryable<Member> GetMembersBySearchParams(int pageNumber, int pageSize, InputMember newMember)
        {
            var result = CustomUtility.FilterMembersBySearchParams ( _context, newMember, pageNumber, pageSize);

            if (result.IsNullOrEmpty())
            {
                throw new Exception("No Members Found");
            }
            
            return result;
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
