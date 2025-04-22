CREATE TABLE Category (
    CategoryId INT PRIMARY KEY,
    CategoryName TEXT
);

CREATE TABLE Item (
    ItemId INT PRIMARY KEY,
    ItemName TEXT,
    CategoryId INT,
    FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
);

CREATE TABLE Orders ( --'Order' is a reserved word :/
    OrderId INT PRIMARY KEY,
    StudentId INT,
    StudentName TEXT,
    OrderedItems TEXT
    OrderDateTime TEXT
);
