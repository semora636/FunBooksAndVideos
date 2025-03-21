using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Customers;
using MediatR;

namespace Kata.Presentation.Handlers.Customers
{
    public class AddCustomerHandler : IRequestHandler<AddCustomerRequest, Customer>
    {
        private readonly ICustomerService _customerService;

        public AddCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Customer> Handle(AddCustomerRequest request, CancellationToken cancellationToken)
        {
            await _customerService.AddCustomerAsync(request.Customer);
            return request.Customer;
        }
    }
}
