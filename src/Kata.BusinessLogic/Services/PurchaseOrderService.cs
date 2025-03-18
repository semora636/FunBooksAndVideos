using Kata.BusinessLogic.Interfaces;
using Kata.DataAccess;
using Kata.DataAccess.Interfaces;
using Kata.Domain.Entities;
using Microsoft.Data.SqlClient;

namespace Kata.BusinessLogic.Services
{
    public class PurchaseOrderService : IPurchaseOrderService
    {
        private readonly IPurchaseOrderRepository _purchaseOrderRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IMembershipProductRepository _membershipProductRepository;
        private readonly SqlDataAccess _dataAccess;

        public PurchaseOrderService(SqlDataAccess dataAccess, IPurchaseOrderRepository purchaseOrderRepository, IOrderItemRepository orderItemRepository, IMembershipRepository membershipRepository, IMembershipProductRepository membershipProductRepository)
        {
            _dataAccess = dataAccess;
            _purchaseOrderRepository = purchaseOrderRepository;
            _orderItemRepository = orderItemRepository;
            _membershipRepository = membershipRepository;
            _membershipProductRepository = membershipProductRepository;
        }

        public PurchaseOrder? GetPurchaseOrderById(int purchaseOrderId)
        {
            var purchaseOrder = _purchaseOrderRepository.GetPurchaseOrderById(purchaseOrderId);

            if (purchaseOrder != null)
            {
                purchaseOrder.Items = _orderItemRepository.GetOrderItemsByPurchaseOrderId(purchaseOrderId);
            }

            return purchaseOrder;
        }

        public IEnumerable<PurchaseOrder> GetAllPurchaseOrders()
        {
            var purchaseOrders = _purchaseOrderRepository.GetAllPurchaseOrders();

            foreach (var purchaseOrder in purchaseOrders)
            {
                purchaseOrder.Items = _orderItemRepository.GetOrderItemsByPurchaseOrderId(purchaseOrder.PurchaseOrderId);
            }

            return purchaseOrders;
        }

        public void AddPurchaseOrder(PurchaseOrder purchaseOrder)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                if (purchaseOrder.Items != null)
                {
                    purchaseOrder.OrderDateTime = DateTime.UtcNow;
                    purchaseOrder.TotalPrice = purchaseOrder.Items.Sum(item => item.Price * item.Quantity);
                    purchaseOrder.PurchaseOrderId = _purchaseOrderRepository.AddPurchaseOrder(purchaseOrder, transaction, connection);

                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = _orderItemRepository.AddOrderItem(item, transaction, connection);

                        if (item.ProductType == Domain.Enums.ProductType.Membership)
                        {
                            // If the item is a membership product, we need to add it to Membership table
                            ActivateMembership(purchaseOrder, connection, transaction, item);
                        }
                        else if (item.ProductType == Domain.Enums.ProductType.Book)
                        {
                            // If the item is a phisical product, so we need to generate a shipping slip
                        }
                    }
                }
                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdatePurchaseOrder(PurchaseOrder purchaseOrder)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                _purchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder, transaction, connection);
                _orderItemRepository.DeleteOrderItemsByPurchaseOrderId(purchaseOrder.PurchaseOrderId, transaction, connection);

                if (purchaseOrder.Items != null)
                {
                    foreach (var item in purchaseOrder.Items)
                    {
                        item.PurchaseOrderId = purchaseOrder.PurchaseOrderId;
                        item.OrderItemId = _orderItemRepository.AddOrderItem(item, transaction, connection);
                    }
                }

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public void DeletePurchaseOrder(int purchaseOrderId)
        {
            using var connection = _dataAccess.CreateConnection();
            connection.Open();
            using var transaction = connection.BeginTransaction();

            try
            {
                _orderItemRepository.DeleteOrderItemsByPurchaseOrderId(purchaseOrderId, transaction, connection);
                _purchaseOrderRepository.DeletePurchaseOrder(purchaseOrderId, transaction, connection);

                transaction.Commit();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        private void ActivateMembership(PurchaseOrder purchaseOrder, SqlConnection connection, SqlTransaction transaction, OrderItem item)
        {
            // TODO: We can check if the user already has an active memvbership and in this case just increase the
            // existing expiration, or add the duration on top of the existing one
            var membershipProduct = _membershipProductRepository.GetMembershipProductById(item.ProductId);
            if (membershipProduct != null)
            {
                var membership = new Membership()
                {
                    ActivationDateTime = DateTime.UtcNow,
                    ExpirationDateTime = DateTime.UtcNow.AddMonths(membershipProduct.DurationMonths),
                    CustomerId = purchaseOrder.CustomerId,
                    MembershipType = membershipProduct.MembershipType
                };
                _membershipRepository.AddMembership(membership, transaction, connection);
            }
        }
    }
}
