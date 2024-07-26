using LMS2.Models;

namespace LMS2.Repository
{
    public interface IMembersRepository
    {
        IEnumerable<Member> GetAllMembers();
        Member GetMemberById(int id);
        void AddMember(Member book);
        Member UpdateMember(int id,Member book);
        Member UpdateMemberByQuery(int id, string? name, string? email, int? mobile, string? address, string? city, string? pincode);
        void DeleteMember(Member member);
        IEnumerable<Member> GetMembersBySearchParams(string? name, string? email, int? mobile, string? city, string? pincode);
        void Save();
    }
}
