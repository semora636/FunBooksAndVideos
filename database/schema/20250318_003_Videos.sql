IF NOT EXISTS (SELECT TOP(1)1 FROM sys.tables WHERE name = 'Videos')
BEGIN
    CREATE TABLE Videos (
        VideoId INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(255) NOT NULL,
        Price DECIMAL(18, 2) NOT NULL,
        Director NVARCHAR(255) NOT NULL
    );
END;