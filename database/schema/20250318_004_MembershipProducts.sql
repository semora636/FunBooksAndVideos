CREATE TABLE MembershipProducts(
    MembershipProductId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    MembershipType INT NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    DurationMonths INT NOT NULL,
);