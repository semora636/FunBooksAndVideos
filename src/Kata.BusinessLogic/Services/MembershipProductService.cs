using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Services
{
    public class MembershipProductService : IMembershipProductService
    {
        private readonly IMembershipProductRepository _membershipProductRepository;

        public MembershipProductService(IMembershipProductRepository membershipProductRepository)
        {
            _membershipProductRepository = membershipProductRepository;
        }

        public async Task<MembershipProduct?> GetMembershipProductByIdAsync(int membershipProductId)
        {
            return await _membershipProductRepository.GetMembershipProductByIdAsync(membershipProductId);
        }

        public async Task<IEnumerable<MembershipProduct>> GetAllMembershipProductsAsync()
        {
            return await _membershipProductRepository.GetAllMembershipProductsAsync();
        }

        public async Task AddMembershipProductAsync(MembershipProduct membershipProduct)
        {
            await _membershipProductRepository.AddMembershipProductAsync(membershipProduct);
        }

        public async Task UpdateMembershipProductAsync(MembershipProduct membershipProduct)
        {
            await _membershipProductRepository.UpdateMembershipProductAsync(membershipProduct);
        }

        public async Task DeleteMembershipProductAsync(int membershipProductId)
        {
            await _membershipProductRepository.DeleteMembershipProductAsync(membershipProductId);
        }
    }
}
