using Kata.Domain.Enums;

namespace Kata.Domain.Entities
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public MembershipType? MembershipType { get; set; }
    }
}
