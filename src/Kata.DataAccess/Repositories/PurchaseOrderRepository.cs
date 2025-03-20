using Dapper;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using System.Data;

namespace Kata.DataAccess.Repositories
{
    public class PurchaseOrderRepository : IPurchaseOrderRepository
    {
        private readonly ISqlDataAccess _dataAccess;
        private readonly IDapperWrapper _dapperWrapper;

        public PurchaseOrderRepository(ISqlDataAccess dataAccess, IDapperWrapper dapperWrapper)
        {
            _dataAccess = dataAccess;
            _dapperWrapper = dapperWrapper;
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderByIdAsync(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryFirstOrDefaultAsync<PurchaseOrder>(connection, "SELECT * FROM PurchaseOrders WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId });
        }

        public async Task<PurchaseOrder?> GetPurchaseOrderWithItemsAndSlipByIdAsync(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();

            var sql = @"
                SELECT 
                    po.*,
                    oi.*,
                    ss.*
                FROM 
                    PurchaseOrders po
                LEFT JOIN 
                    OrderItems oi ON po.PurchaseOrderId = oi.PurchaseOrderId
                LEFT JOIN 
                    ShippingSlips ss ON po.PurchaseOrderId = ss.PurchaseOrderId
                WHERE 
                    po.PurchaseOrderId = @PurchaseOrderId;
            ";

            PurchaseOrder? purchaseOrder = null;

            await connection.QueryAsync<PurchaseOrder, OrderItem, ShippingSlip, PurchaseOrder>(
                sql,
                (po, oi, ss) =>
                {
                    if (purchaseOrder == null)
                    {
                        purchaseOrder = po;
                    }

                    if (oi != null)
                    {
                        purchaseOrder.Items ??= [];
                        if (!purchaseOrder.Items.Any(o => o.OrderItemId == oi.OrderItemId)) // Replace 'Id' with your unique identifier
                        {
                            purchaseOrder.Items.Add(oi);
                        }
                    }

                    if (ss != null)
                    {
                        purchaseOrder.ShippingSlips ??= [];
                        if (!purchaseOrder.ShippingSlips.Any(s => s.ShippingSlipId == ss.ShippingSlipId))
                        {
                            purchaseOrder.ShippingSlips.Add(ss);
                        }
                    }

                    return purchaseOrder;
                },
                new { PurchaseOrderId = purchaseOrderId },
                splitOn: "PurchaseOrderId,OrderItemId,ShippingSlipId"
            );

            return purchaseOrder;
        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersAsync()
        {
            using var connection = _dataAccess.CreateConnection();
            return await _dapperWrapper.QueryAsync<PurchaseOrder>(connection, "SELECT * FROM PurchaseOrders");

        }

        public async Task<IEnumerable<PurchaseOrder>> GetAllPurchaseOrdersWithItemsAndSlipsAsync()
        {
            using var connection = _dataAccess.CreateConnection();

            var sql = @"
                SELECT 
                    po.*,
                    oi.*,
                    ss.*
                FROM 
                    PurchaseOrders po
                LEFT JOIN 
                    OrderItems oi ON po.PurchaseOrderId = oi.PurchaseOrderId
                LEFT JOIN 
                    ShippingSlips ss ON po.PurchaseOrderId = ss.PurchaseOrderId;
            ";

            var purchaseOrderDictionary = new Dictionary<int, PurchaseOrder>();

            var result = await connection.QueryAsync<PurchaseOrder, OrderItem, ShippingSlip, PurchaseOrder>(
                sql,
                (po, oi, ss) =>
                {
                    if (!purchaseOrderDictionary.TryGetValue(po.PurchaseOrderId, out var purchaseOrder))
                    {
                        purchaseOrder = po;
                        purchaseOrderDictionary.Add(purchaseOrder.PurchaseOrderId, purchaseOrder);
                    }

                    if (oi != null)
                    {
                        purchaseOrder.Items ??= [];
                        if (!purchaseOrder.Items.Any(o => o.OrderItemId == oi.OrderItemId)) // Replace 'Id' with your unique identifier
                        {
                            purchaseOrder.Items.Add(oi);
                        }
                    }

                    if (ss != null)
                    {
                        purchaseOrder.ShippingSlips ??= [];
                        if (!purchaseOrder.ShippingSlips.Any(s => s.ShippingSlipId == ss.ShippingSlipId))
                        {
                            purchaseOrder.ShippingSlips.Add(ss);
                        }
                    }

                    return purchaseOrder;
                },
                splitOn: "PurchaseOrderId,OrderItemId,ShippingSlipId"
            );

            return purchaseOrderDictionary.Values;
        }

        public async Task<int> AddPurchaseOrderAsync(PurchaseOrder purchaseOrder, IDbTransaction transaction, IDbConnection connection)
        {
            return await _dapperWrapper.ExecuteScalarAsync<int>(connection, "INSERT INTO PurchaseOrders (CustomerId, OrderDateTime, TotalPrice) VALUES (@CustomerId, @OrderDateTime, @TotalPrice); SELECT SCOPE_IDENTITY();", purchaseOrder, transaction);
        }

        public async Task UpdatePurchaseOrderAsync(PurchaseOrder purchaseOrder, IDbTransaction transaction, IDbConnection connection)
        {
            await _dapperWrapper.ExecuteAsync(connection, "UPDATE PurchaseOrders SET CustomerId = @CustomerId, OrderDateTime = @OrderDateTime, TotalPrice = @TotalPrice WHERE PurchaseOrderId = @PurchaseOrderId", purchaseOrder, transaction);
        }

        public async Task DeletePurchaseOrderAsync(int purchaseOrderId, IDbTransaction transaction, IDbConnection connection)
        {
            await _dapperWrapper.ExecuteAsync(connection, "DELETE FROM OrderItems WHERE PurchaseOrderId = @PurchaseOrderId", new { PurchaseOrderId = purchaseOrderId }, transaction);
        }
    }
}
