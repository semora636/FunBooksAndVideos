using Kata.BusinessLogic.Interfaces;
using Kata.Presentation.Requests.Customers;
using MediatR;

namespace Kata.Presentation.Handlers.Customers
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerRequest>
    {
        private readonly ICustomerService _customerService;

        public DeleteCustomerHandler(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
        {
            _ = await _customerService.GetCustomerByIdAsync(request.Id) ?? throw new KeyNotFoundException($"Customer with ID {request.Id} not found.");
            await _customerService.DeleteCustomerAsync(request.Id);
        }
    }
}
