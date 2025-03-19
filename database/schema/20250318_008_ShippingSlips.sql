IF NOT EXISTS (SELECT TOP(1)1 FROM sys.tables WHERE name = 'ShippingSlips')
BEGIN
    CREATE TABLE ShippingSlips(
        ShippingSlipId INT PRIMARY KEY IDENTITY(1,1),
        PurchaseOrderId INT NOT NULL,
        RecipientAddress NVARCHAR(MAX) NOT NULL,
        FOREIGN KEY (PurchaseOrderId) REFERENCES PurchaseOrders(PurchaseOrderId)
    );
END;