namespace Kata.Domain.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set;}
        public required string Email { get; set; }
        public required string Address { get; set; }
        public List<Membership>? Memberships { get; set; }
    }
}
