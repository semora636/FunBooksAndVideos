namespace Kata.Domain.Entities
{
    public class PurchaseOrder
    {
        public int PurchaseOrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDateTime { get; set; }
        public decimal TotalPrice { get; set; }
        public IEnumerable<OrderItem>? Items { get; set; }
        public IEnumerable<ShippingSlip>? ShippingSlips { get; set; }
    }
}
