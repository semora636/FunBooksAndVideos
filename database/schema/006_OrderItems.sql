CREATE TABLE OrderItems (
    OrderItemId INT PRIMARY KEY IDENTITY(1,1),
    PurchaseOrderId INT NOT NULL,
    ProductId INT NOT NULL,
    ProductType INT NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (PurchaseOrderId) References PurchaseOrders(PurchaseOrderId)
);