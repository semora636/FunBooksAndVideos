using Kata.Domain.Entities;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IMembershipService
    {
        IEnumerable<Membership> GetMembershipsByCustomerId(int customerId);
    }
}
