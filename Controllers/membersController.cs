using LMS2.DataContext;
using LMS2.Models;
using LMS2.Models.ViewModels;
using LMS2.Repository;
using LMS2.Utility;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace LMS2.Controllers
{
    
    
    
    /// <summary>
    /// Member Routes
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMembersRepository _membersRepository;
        
        
        
        /// <summary>
        /// Member Controlller
        /// </summary>
        /// <param name="membersRepository"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MembersController(IMembersRepository membersRepository)
        {
            if (membersRepository != null)
                _membersRepository = membersRepository;
            else
                throw new ArgumentNullException(nameof(membersRepository));
        }



        /// <summary>
        /// Get All Member Data
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public JsonResult Get()
        {
            try
            {
                var res = _membersRepository.GetAllMembers();
                return new JsonResult(new { data = res.ToList() });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }



        /// <summary>
        /// Get Member Data By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public JsonResult GetByID(int id)
        {
            try
            {
                var res = _membersRepository.GetMemberById(id);
                return new JsonResult(new { data = res });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
        
        
        
        /// <summary>
        /// Add New Member
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMember(InputMember? member)
        {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(member);
                _membersRepository.AddMember(member);
                _membersRepository.Save();
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
        
        
        /// <summary>
        /// Delete Existing Member by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public JsonResult DeleteMember(int id)
        {
            try
            {
                _membersRepository.DeleteMember(id);
                _membersRepository.Save();
                return new JsonResult(Ok());
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
        
        
        /// <summary>
        /// Patch for updating member by name, email, mobile number, address, city, pincode
        /// </summary>
        /// <param name="id"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public JsonResult PatchMember(int id,InputMember? member)
        {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(member);

                var res = _membersRepository.UpdateMember(id, member);
                _membersRepository.Save();
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }

        
        
        
        /// <summary>
        /// Search member by Name, Email, MobileNumber, City and Pincode
        /// </summary>
        [HttpGet("search")]
        public JsonResult GetMemberBySearch([FromQuery]InputMember inputMember, int pageNumber = 1, int pageSize = int.MaxValue)
        {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(inputMember);

                ValidationUtility.PageInfoValidator(pageNumber, pageSize);

                var res = _membersRepository.GetMembersBySearchParams(pageNumber, pageSize, inputMember);
                _membersRepository.Save();
                return new JsonResult(res);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(ex.Message);
            }
        }
    }
}