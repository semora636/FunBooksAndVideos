namespace Kata.Domain.Entities
{
    public abstract class Product
    {
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}
