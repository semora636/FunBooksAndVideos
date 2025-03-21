using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.Customers;
using MediatR;

namespace Kata.Presentation.Handlers.Customers
{
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerRequest>
    {
        private readonly ICustomerService _customerService;

        public UpdateCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
        {
            await _customerService.UpdateCustomerAsync(request.Customer);
        }
    }
}
