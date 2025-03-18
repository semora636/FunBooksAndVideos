namespace Kata.Domain.Entities
{
    public class Video : IProduct
    {
        public int VideoId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
        public string? Director { get; set; }

        public int ProductId => VideoId;
    }
}
