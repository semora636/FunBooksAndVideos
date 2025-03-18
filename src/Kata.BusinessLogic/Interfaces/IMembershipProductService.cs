using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IMembershipProductService
    {
        MembershipProduct? GetMembershipProductById(int membershipProductId);
        IEnumerable<MembershipProduct> GetAllMembershipProducts();
        void AddMembershipProduct(MembershipProduct membershipProduct);
        void UpdateMembershipProduct(MembershipProduct membershipProduct);
        void DeleteMembershipProduct(int membershipProductId);
    }
}
