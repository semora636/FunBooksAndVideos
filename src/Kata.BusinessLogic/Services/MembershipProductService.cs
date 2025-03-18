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

        public MembershipProduct? GetMembershipProductById(int membershipProductId)
        {
            return _membershipProductRepository.GetMembershipProductById(membershipProductId);
        }

        public IEnumerable<MembershipProduct> GetAllMembershipProducts()
        {
            return _membershipProductRepository.GetAllMembershipProducts();
        }

        public void AddMembershipProduct(MembershipProduct membershipProduct)
        {
            _membershipProductRepository.AddMembershipProduct(membershipProduct);
        }

        public void UpdateMembershipProduct(MembershipProduct membershipProduct)
        {
            _membershipProductRepository.UpdateMembershipProduct(membershipProduct);
        }

        public void DeleteMembershipProduct(int membershipProductId)
        {
            _membershipProductRepository.DeleteMembershipProduct(membershipProductId);
        }
    }
}
