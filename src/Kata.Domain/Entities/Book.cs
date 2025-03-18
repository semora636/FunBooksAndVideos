namespace Kata.Domain.Entities
{
    public class Book : IProduct
    { 
        public int BookId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? Author { get; set; }

        public int ProductId => BookId;
    }
}
