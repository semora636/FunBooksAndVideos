using MediatR;

namespace Kata.Presentation.Requests.Customers
{
    public class DeleteCustomerRequest : IRequest
    {
        public int Id { get; set; }
    }
}
