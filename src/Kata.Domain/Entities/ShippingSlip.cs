namespace Kata.Domain.Entities
{
    public class ShippingSlip
    {
        public int ShippingSlipId { get; set; }
        public int PurchaseOrderId { get; set; }
        public required string RecipientAddress { get; set; }
    }
}
