IF NOT EXISTS (SELECT TOP(1)1 FROM sys.tables WHERE name = 'Customers')
BEGIN
    CREATE TABLE Customers (
        CustomerId INT PRIMARY KEY IDENTITY(1,1),
        FirstName NVARCHAR(255),
        LastName NVARCHAR(255),
        EmailAddress NVARCHAR(255) NOT NULL,
        Address NVARCHAR(255) NOT NULL
    );
END;