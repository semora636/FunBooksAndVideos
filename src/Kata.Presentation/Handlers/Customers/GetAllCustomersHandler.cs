using Kata.BusinessLogic.Interfaces;
using Kata.Domain.Entities;
using Kata.Presentation.Requests.Customers;
using MediatR;

namespace Kata.Presentation.Handlers.Customers
{
    public class GetAllCustomersHandler : IRequestHandler<GetAllCustomersRequest, IEnumerable<Customer>>
    {
        private readonly ICustomerService _customerService;

        public GetAllCustomersHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IEnumerable<Customer>> Handle(GetAllCustomersRequest request, CancellationToken cancellationToken)
        {
            return await _customerService.GetAllCustomersAsync();
        }
    }
}
