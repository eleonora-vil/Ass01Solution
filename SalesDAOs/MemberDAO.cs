using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SalesBusinessObjects;

namespace SalesDAOs
{
    public class MemberDAO
    {
        private readonly FStoreContext db = null;
        private static MemberDAO instance = null;

        public MemberDAO()
        {
            db = new FStoreContext();
        }

        public static MemberDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MemberDAO();
                }
                return instance;
            }
        }
        public Member GetAccount(string email, string password)
        {
            return db.Members.FirstOrDefault(x => x.Email.Equals(email) && x.Password.Equals(password));
        }

        public Member GetMember(int memberID)
        {
            return db.Members.FirstOrDefault(x => x.MemberId == memberID);
        }
        public List<Member> GetMembers()
        {
            return db.Members.Select(m => new Member
            {
                MemberId = m.MemberId,
                Email = m.Email,
                CompanyName = m.CompanyName,
                City = m.City,
                Country = m.Country,
                Password = m.Password
            }).ToList();
        }

        public void DeleteMember(int memberID)
        {
            try
            {
                Member member = GetMember(memberID);
                if (member != null)
                {
                    // First, remove OrderDetails associated with Orders of the Member
                    var ordersToDelete = db.Orders.Where(o => o.MemberId == memberID).ToList();
                    foreach (var order in ordersToDelete)
                    {
                        var orderDetailsToDelete = db.OrderDetails.Where(od => od.OrderId == order.OrderId).ToList();
                        foreach (var orderDetail in orderDetailsToDelete)
                        {
                            db.OrderDetails.Remove(orderDetail);
                        }

                        db.Orders.Remove(order);
                    }

                    // Then, remove the Member
                    db.Members.Remove(member);
                    db.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.InnerException as SqlException;
                if (sqlException != null)
                {
                    foreach (SqlError error in sqlException.Errors)
                    {
                        Console.WriteLine($"SQL Error: {error.Message}");
                    }
                }
                throw; // Re-throw the exception after logging
            }
        }

        public void UpdateMember(int memberID, Member newMember)
        {
            var existingMember = GetMember(memberID);
            if (existingMember != null)
            {
                existingMember.CompanyName = newMember.CompanyName;
                existingMember.City = newMember.City;
                existingMember.Country = newMember.Country;
                existingMember.Password = newMember.Password;

                db.Members.Attach(existingMember);
                db.Entry(existingMember).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void AddMember(Member member)
        {
            Member newMember = GetMember(member.MemberId);
            if (newMember == null)
            {
                db.Members.Add(member);
                db.SaveChanges();
            }
        }
    }
}
