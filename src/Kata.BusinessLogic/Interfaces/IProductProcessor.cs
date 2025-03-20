using Kata.Domain.Entities;
using Kata.Domain.Enums;
using System.Data;

namespace Kata.BusinessLogic.Interfaces
{
    public interface IProductProcessor
    {
        Task ProcessProductAsync(PurchaseOrder purchaseOrder, OrderItem item, IDbConnection connection, IDbTransaction transaction);
        ProductType ProductType { get; }
    }
}
