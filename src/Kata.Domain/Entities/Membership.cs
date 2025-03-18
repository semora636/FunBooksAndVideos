using Kata.Domain.Enums;

namespace Kata.Domain.Entities
{
    public class Membership
    {
        public int MembershipId { get; set; }
        public MembershipType MembershipType { get; set; }
        public DateTime ActivationDateTime { get; set; }
        public DateTime ExpirationDateTime { get; set; }
        public int CustomerId { get; set; }
    }
}
