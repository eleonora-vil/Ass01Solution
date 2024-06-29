using SalesBusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{

    public interface IMemberRepository
    {
        public Member GetAccount(string email, string password);
        public Member GetMember(int memberID);
        public List<Member> GetMembers();
        public void DeleteMember(int memberID);
        public void UpdateMember(int memberID, Member newMember);
        public void AddMember(Member member);
    }
}
