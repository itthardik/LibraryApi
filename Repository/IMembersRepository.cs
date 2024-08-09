using LMS2.Models;
using LMS2.Models.ViewModels.Request;
using LMS2.Models.ViewModels.Search;
using Microsoft.AspNetCore.Mvc;

namespace LMS2.Repository
{
    /// <summary>
    /// Interface of Member Repo
    /// </summary>
    public interface IMembersRepository
    {
        /// <summary>
        /// get all member in json with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        JsonResult GetAllMemberByPagination(int pageNumber, int pageSize);
        /// <summary>
        /// get member by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Member GetMemberById(int id);
        /// <summary>
        /// add new member
        /// </summary>
        /// <param name="member"></param>
        void AddMember(RequestMember? member);
        /// <summary>
        /// update member
        /// </summary>
        /// <param name="id"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        Member UpdateMember(int id,RequestMember member);
        /// <summary>
        /// delete member
        /// </summary>
        /// <param name="id"></param>
        void DeleteMember(int id);
        /// <summary>
        /// get member by search params
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="newMember"></param>
        /// <returns></returns>
        JsonResult GetMembersBySearchParams(int pageNumber, int pageSize, SearchMember newMember);
        /// <summary>
        /// save db
        /// </summary>
        void Save();
    }
}
