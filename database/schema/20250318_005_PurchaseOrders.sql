IF NOT EXISTS (SELECT TOP(1)1 FROM sys.tables WHERE name = 'PurchaseOrders')
BEGIN
    CREATE TABLE PurchaseOrders (
        PurchaseOrderId INT PRIMARY KEY IDENTITY(1,1),
        CustomerId INT NOT NULL,
        OrderDateTime DATETIME NOT NULL,
        TotalPrice DECIMAL(18, 2) NOT NULL,
        FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
    );
END;