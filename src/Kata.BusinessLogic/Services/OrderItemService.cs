﻿using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId)
        {
            return await _orderItemRepository.GetOrderItemsByPurchaseOrderIdAsync(purchaseOrderId);
        }

        public async Task<int> AddOrderItemAsync(OrderItem orderItem, SqlTransaction transaction, SqlConnection connection)
        {
            return await _orderItemRepository.AddOrderItemAsync(orderItem, transaction, connection);
        }

        public async Task DeleteOrderItemsByPurchaseOrderIdAsync(int purchaseOrderId, SqlTransaction transaction, SqlConnection connection)
        {
            await _orderItemRepository.DeleteOrderItemsByPurchaseOrderIdAsync(purchaseOrderId, transaction, connection);
        }
    }
}
