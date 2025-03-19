IF NOT EXISTS (SELECT TOP(1)1 FROM sys.tables WHERE name = 'Books')
BEGIN
    CREATE TABLE Books (
        BookId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(255) NOT NULL,
        Price DECIMAL(18, 2) NOT NULL,
        Author NVARCHAR(255) NOT NULL
    );
END;