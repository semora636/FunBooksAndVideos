-- Insert Customers
INSERT INTO Customers (FirstName, LastName, EmailAddress, Address) VALUES
('John', 'Doe', 'john.doe@example.com', '123 Main St'),
('Jane', 'Smith', 'jane.smith@example.com', '456 Oak Ave'),
('Alice', 'Johnson', 'alice.johnson@example.com', '789 Pine Ln'),
('Bob', 'Williams', 'bob.williams@example.com', '101 Elm Rd');

-- Insert Books
INSERT INTO Books (Name, Price, Author) VALUES
('The Girl on the train', 15.99, 'Paula Hawkins'),
('The Silent Patient', 12.50, 'Alex Michaelides'),
('Where the Crawdads Sing', 14.75, 'Delia Owens');

-- Insert Videos
INSERT INTO Videos (Name, Price, Director) VALUES
('Comprehensive First Aid Training', 25.00, 'Dr. Smith'),
('Introduction to Cooking', 19.99, 'Chef Jones'),
('Advanced Guitar Techniques', 30.00, 'Guitar Master');

-- Insert MembershipProducts
INSERT INTO MembershipProducts (Name, MembershipType, Price, DurationMonths) VALUES
('Book Club Membership', 1, 10.00, 12),
('Video Club Membership', 2, 12.00, 12),
('Premium Membership', 3, 20.00, 12);