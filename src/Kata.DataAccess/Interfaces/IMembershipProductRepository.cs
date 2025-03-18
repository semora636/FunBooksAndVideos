using Kata.Domain.Entities;

namespace Kata.DataAccess.Interfaces
{
    public interface IMembershipProductRepository
    {
        MembershipProduct? GetMembershipProductById(int membershipProductId);
        IEnumerable<MembershipProduct> GetAllMembershipProducts();
        void AddMembershipProduct(MembershipProduct membershipProduct);
        void UpdateMembershipProduct(MembershipProduct membershipProduct);
        void DeleteMembershipProduct(int membershipProductId);
    }
}
