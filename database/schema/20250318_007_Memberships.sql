CREATE TABLE Memberships(
    MembershipId int primary key identity(1,1),
    CustomerId int not null,
    MembershipType int not null,
    ActivationDateTime datetime not null,
    ExpirationDateTime datetime not null,
    Foreign Key (CustomerId) references Customers(CustomerID)
)