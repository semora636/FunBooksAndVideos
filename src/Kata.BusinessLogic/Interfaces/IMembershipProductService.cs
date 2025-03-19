using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IMembershipProductService
    {
        Task<MembershipProduct?> GetMembershipProductByIdAsync(int membershipProductId);
        Task<IEnumerable<MembershipProduct>> GetAllMembershipProductsAsync();
        Task AddMembershipProductAsync(MembershipProduct membershipProduct);
        Task UpdateMembershipProductAsync(MembershipProduct membershipProduct);
        Task DeleteMembershipProductAsync(int membershipProductId);
    }
}
