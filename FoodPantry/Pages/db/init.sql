CREATE TABLE Item
(
    ItemId INT,
    ItemName TEXT,
    FOREIGN KEY CategoryId INT REFERENCES Category(CategoryId)
);

CREATE TABLE Category
(
    CategoryId INT,
    CategoryName
);

CREATE TABLE Order
(
    OrderId INT,
    StudentId INT,
    StudentName TEXT,
    ItemsList TEXT, --- do i need to normalize this?
    OrderDateTime TEXT
);