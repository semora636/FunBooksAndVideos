namespace Kata.Domain.Entities
{
    public interface IProduct
    {
        public int ProductId { get; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
