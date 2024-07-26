using LMS2.DataContext;
using LMS2.Models;
using LMS2.Repository;
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
    public class membersController : ControllerBase
    {
        private readonly IMembersRepository _membersRepository;
        public membersController(IMembersRepository membersRepository)
        {
            _membersRepository = membersRepository;
        }
        /// <summary>
        /// Get All Member Data
        /// </summary>
        [HttpGet]
        public JsonResult Get()
        {
            var res = _membersRepository.GetAllMembers();
            if (res.IsNullOrEmpty())
            {
                return new JsonResult(new { message = "The array is empty" });
            }
            return new JsonResult(new { data = res });
        }
        /// <summary>
        /// Get Member Data By Id
        /// </summary>
        [HttpGet("{id}")]
        public JsonResult GetByID(int id)
        {
            var res = _membersRepository.GetMemberById(id);
            if (res == null)
            {
                return new JsonResult(new { message = "No member found with this Id" });
            }
            return new JsonResult(new { data = res });
        }
        /// <summary>
        /// Add New Member
        /// </summary>
        [HttpPost]
        public JsonResult AddMember(Member member)
        {
            var check = _membersRepository.GetAllMembers()
                            .Where(x => (
                                x.Name == member.Name) &&
                                (x.Email == member.Email) &&
                                (x.MobileNumber == member.MobileNumber)
                            );
            if (!check.IsNullOrEmpty())
                return new JsonResult(new { message = "Member with this name, email and mobile number already existed" });

            try
            {
                _membersRepository.AddMember(member);
            }
            catch (Exception ex)
            {
                return new JsonResult(new { errorMessage = ex.Message });
            }
            _membersRepository.Save();
            return new JsonResult(Ok());
        }
        /// <summary>
        /// Delete Existing Member by Id
        /// </summary>
        [HttpDelete]
        public JsonResult DeleteMember(int id)
        {
            var member = _membersRepository.GetMemberById(id);
            if (member == null)
            {
                return new JsonResult(new { message = "No member found with this Id" });
            }
            _membersRepository.DeleteMember(member);
            _membersRepository.Save();
            return new JsonResult(Ok());
        }
        /// <summary>
        /// Update whole Member Data
        /// </summary>
        [HttpPut("{id}")]
        public JsonResult PutMember(int id, Member member)
        {
            if (id == null)
            {
                return new JsonResult(new { message = "Id parameter is required" });
            }
            var res = _membersRepository.UpdateMember(id, member);
            if (res == null)
            {
                return new JsonResult(new { message = "No Member Found with this id" });
            }
            _membersRepository.Save();
            return new JsonResult(res);
        }
        /// <summary>
        /// Patch for updating member by name, email, mobile number, address, city, pincode
        /// </summary>
        [HttpPatch("{id}")]
        public JsonResult PatchMember(int id, string? name, string? email, int? mobile, string? address, string? city, string? pincode)
        {
            if (id == null)
            {
                return new JsonResult(new { message = "Id parameter is required" });
            }
            if (name == null && email == null && mobile == null && address == null && city == null)
            {
                return new JsonResult(new { message = "atleast one field is required to patch member" });
            }
            if (name?.Length >= 300 ||
                email?.Length >= 100 ||
                ! new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").IsMatch(email??"") ||
                ! new Regex(@"^\d{10}$").IsMatch(mobile?.ToString()??"")||
                ! new Regex(@"^\d{6}$").IsMatch(pincode ?? "")
                )
            {
                return new JsonResult(new { message = "Invalid format" });
            }
            var res = _membersRepository.UpdateMemberByQuery(id, name, email, mobile, address, city, pincode);
            if (res == null)
            {
                return new JsonResult(new { message = "No Member Found with this id" });
            }
            _membersRepository.Save();
            return new JsonResult(res);
        }

        /// <summary>
        /// Search member by Name, Email, MobileNumber, City and Pincode
        /// </summary>
        [HttpGet("search")]
        public JsonResult GetMemberBySearch(string? name, string? email, int? mobile, string? city , string? pincode)
        {
            if (name == null && email == null && mobile == null && city == null && pincode == null)
                return new JsonResult(new { message = "provide atleast one params" });

            var res = _membersRepository.GetMembersBySearchParams(name, email, mobile, city, pincode);

            if (res.IsNullOrEmpty())
                return new JsonResult(new { message = "No Member Found" });

            return new JsonResult(new { message = res });
        }
    }
}