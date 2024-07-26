using LMS2.DataContext;
using LMS2.Models;

namespace LMS2.Repository
{
    public class MembersRepository : IMembersRepository
    {
        private readonly ApiContext _context;
        public MembersRepository(ApiContext context)
        {
            _context = context;
        }

        public IEnumerable<Member> GetAllMembers()
        {
            return _context.members.ToList();
        }
        public Member? GetMemberById(int id)
        {
            return _context.members.Find(id);
        }
        public void AddMember(Member book)
        {
            _context.members.Add(book);
            return;
        }
        public void DeleteMember(Member member)
        {
            _context.members.Remove(member);
            return;
        }
        public Member UpdateMember(int id, Member book)
        {
            try
            {
                var foundMember = _context.members.ToList().Find(m => m.Id == id);
                if (foundMember != null)
                {
                    foundMember.Name = book.Name;
                    foundMember.Email = book.Email;
                    foundMember.MobileNumber = book.MobileNumber;
                    foundMember.Address = book.Address;
                    foundMember.City = book.City;
                    foundMember.Pincode = book.Pincode;

                    return foundMember;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        public Member UpdateMemberByQuery(int id, string? name, string? email, int? mobile, string? address, string? city, string? pincode)
        {
            try
            {
                var foundMember = _context.members.ToList().Find(m => m.Id == id);
                if (foundMember != null)
                {
                    foundMember.Name = name ?? foundMember.Name;
                    foundMember.Email = email ?? foundMember.Email;
                    foundMember.MobileNumber = mobile ?? foundMember.MobileNumber;
                    foundMember.Address = address ?? foundMember.Address;
                    foundMember.City = city ?? foundMember.City;
                    foundMember.Pincode = pincode ?? foundMember.Pincode;

                    return foundMember;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        public IEnumerable<Member> GetMembersBySearchParams(string? name, string? email, int? mobile, string? city, string? pincode)
        {
            var allMembers = _context.members.ToList();
            var result = allMembers.FindAll(a =>
                            (name != null && a.Name.Contains(name)) ||
                            (email != null && a.Email.Contains(email)) ||
                            (mobile != null && a.MobileNumber.ToString().Contains(mobile.ToString())) ||
                            (city != null && a.City.Contains(city)) ||
                            (pincode != null && a.Pincode.ToString().Contains(pincode.ToString()))
                        );
            return result;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
