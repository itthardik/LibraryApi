using LMS2.Models.ViewModels.Request;
using LMS2.Models.ViewModels.Search;
using LMS2.Repository;
using LMS2.Utility;
using Microsoft.AspNetCore.Mvc;

namespace LMS2.Controllers
{



    /// <summary>
    /// Member Routes
    /// </summary>
    [Route("api/members")]
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
        public JsonResult Get(int pageNumber, int pageSize)
        {
            try
            {
                ValidationUtility.PageInfoValidator(pageNumber, pageSize);
                var res = _membersRepository.GetAllMemberByPagination(pageNumber, pageSize);
                return res;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message, type = ex.GetType().ToString() });
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
                return new JsonResult(new { error = ex.Message, type = ex.GetType().ToString() });
            }
        }
        
        
        
        /// <summary>
        /// Add New Member
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddMember(RequestMember? member)
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
                return new JsonResult(new { error = ex.Message, type = ex.GetType().ToString() });
            }
        }


        /// <summary>
        /// Delete Existing Member by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
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
                return new JsonResult(new { error = ex.Message, type = ex.GetType().ToString() });
            }
        }
        
        
        /// <summary>
        /// Patch for updating member by name, email, mobile number, address, city, pincode
        /// </summary>
        /// <param name="id"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public JsonResult PatchMember(int id,RequestMember member)
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
                return new JsonResult(new { error = ex.Message, type = ex.GetType().ToString() });
            }
        }

        
        
        
        /// <summary>
        /// Search member by Name, Email, MobileNumber, City and Pincode
        /// </summary>
        [HttpGet("search")]
        public JsonResult GetMemberBySearch([FromQuery]SearchMember requestMember, int pageNumber = 1, int pageSize = int.MaxValue)
        {
            try
            {
                ValidationUtility.ObjectIsNullOrEmpty(requestMember);

                ValidationUtility.PageInfoValidator(pageNumber, pageSize);

                var res = _membersRepository.GetMembersBySearchParams(pageNumber, pageSize, requestMember);
                
                return res;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return new JsonResult(new { error = ex.Message, type = ex.GetType().ToString() });
            }
        }
    }
}