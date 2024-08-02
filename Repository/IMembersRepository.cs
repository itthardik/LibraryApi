using LMS2.Models;
using LMS2.Models.ViewModels;

namespace LMS2.Repository
{
    /// <summary>
    /// Interface of Member Repo
    /// </summary>
    public interface IMembersRepository
    {
        /// <summary>
        /// 
        /// </summary>
        IQueryable<Member> GetAllMembers();
        /// <summary>
        /// 
        /// </summary>
        Member GetMemberById(int id);
        /// <summary>
        /// 
        /// </summary>
        void AddMember(RequestMember? member);
        /// <summary>
        /// 
        /// </summary>
        Member UpdateMember(int id,RequestMember? member);
        /// <summary>
        /// 
        /// </summary>
        void DeleteMember(int id);
        /// <summary>
        /// 
        /// </summary>
        IQueryable<Member> GetMembersBySearchParams(int pageNumber, int pageSize, RequestMember newMember);
        /// <summary>
        /// 
        /// </summary>
        void Save();
    }
}
