using Kata.Domain.Enums;

namespace Kata.Domain.Entities
{
    public class MembershipProduct : IProduct
    { 
        public int MembershipProductId { get; set; }
        public required string Name { get; set; }
        public MembershipType Type { get; set; }
        public decimal Price { get; set; }
        public int DurationMonths { get; set; }

        public int ProductId => MembershipProductId;
    }
}
