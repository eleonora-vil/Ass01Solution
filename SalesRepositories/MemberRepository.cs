using SalesBusinessObjects;
using SalesDAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesRepositories
{
    public class MemberRepository : IMemberRepository
    {
        private readonly MemberDAO memberDAO = null;

        public MemberRepository()
        {
            if (memberDAO == null)
            {
                memberDAO = new MemberDAO();
            }
        }
        public void AddMember(Member member)
        => MemberDAO.Instance.AddMember(member);

        public void DeleteMember(int memberID)
        => MemberDAO.Instance.DeleteMember(memberID);

        public Member GetAccount(string email, string password)
        {
            return memberDAO.GetAccount(email, password);
        }

        public Member GetMember(int memberID)
                => MemberDAO.Instance.GetMember(memberID);


        public List<Member> GetMembers()
                => MemberDAO.Instance.GetMembers();


        public void UpdateMember(int memberID, Member newMember)
                => MemberDAO.Instance.UpdateMember(memberID, newMember);

    }
}
