-- MySQL Database Schema for CRMProductSystem
-- Generated from C# Models: User, Customer, Product, TaskModel, TaskFollowup, Role

CREATE DATABASE IF NOT EXISTS CRMProductSystem;
USE CRMProductSystem;

-- Roles Table
CREATE TABLE Roles (
    RoleId INT AUTO_INCREMENT PRIMARY KEY,
    RoleName VARCHAR(50) NOT NULL UNIQUE
) ENGINE=InnoDB;

INSERT INTO Roles (RoleName) VALUES ('Admin'), ('Employee');

-- Users Table
CREATE TABLE Users (
    UserId INT AUTO_INCREMENT PRIMARY KEY,
    Username VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    Role VARCHAR(50) NOT NULL DEFAULT 'Employee',
    RoleId INT NULL,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    INDEX idx_username (Username)
) ENGINE=InnoDB;

-- Customers Table
CREATE TABLE Customers (
    CustomerId INT AUTO_INCREMENT PRIMARY KEY,
    CustomerName VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    Email VARCHAR(100),
    Address TEXT,
    INDEX idx_customername (CustomerName),
    INDEX idx_email (Email)
) ENGINE=InnoDB;

-- Products Table
CREATE TABLE Products (
    ProductId INT AUTO_INCREMENT PRIMARY KEY,
    ProductName VARCHAR(255) NOT NULL,
    Price DECIMAL(10,2) NOT NULL DEFAULT 0.00,
    Description TEXT,
    INDEX idx_productname (ProductName)
) ENGINE=InnoDB;

-- Tasks Table
CREATE TABLE Tasks (
    TaskId INT AUTO_INCREMENT PRIMARY KEY,
    Title VARCHAR(255) NOT NULL,
    Description TEXT,
    Status ENUM('Pending', 'In Progress', 'Completed') DEFAULT 'Pending',
    DueDate DATETIME NULL,
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    CustomerId INT NULL,
    ProductId INT NULL,
    AssignedTo INT NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId) ON DELETE SET NULL,
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId) ON DELETE SET NULL,
    FOREIGN KEY (AssignedTo) REFERENCES Users(UserId) ON DELETE SET NULL,
    INDEX idx_status (Status),
    INDEX idx_duedate (DueDate),
    INDEX idx_assignedto (AssignedTo)
) ENGINE=InnoDB;

-- TaskFollowups Table
CREATE TABLE TaskFollowups (
    FollowupId INT AUTO_INCREMENT PRIMARY KEY,
    TaskId INT NOT NULL,
    Notes TEXT,
    FollowupDate DATETIME DEFAULT CURRENT_TIMESTAMP,
    SalesStatus ENUM('Interested', 'Not Interested', 'Purchased') DEFAULT 'Interested',
    UserId INT NOT NULL,
    FOREIGN KEY (TaskId) REFERENCES Tasks(TaskId) ON DELETE CASCADE,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    INDEX idx_taskid (TaskId),
    INDEX idx_userid (UserId)
) ENGINE=InnoDB;

-- Usage: mysql -u root -p CRMProductSystem < schema.


USE CRMProductSystem;

-- =========================================
-- SAMPLE USERS (10 records)
-- =========================================
INSERT INTO Users (Username, Password, Role, RoleId) VALUES
('admin', 'admin123', 'Admin', 1),
('employee1', 'emp123', 'Employee', 2),
('employee2', 'emp123', 'Employee', 2),
('employee3', 'emp123', 'Employee', 2),
('employee4', 'emp123', 'Employee', 2),
('employee5', 'emp123', 'Employee', 2),
('employee6', 'emp123', 'Employee', 2),
('employee7', 'emp123', 'Employee', 2),
('employee8', 'emp123', 'Employee', 2),
('employee9', 'emp123', 'Employee', 2);

-- =========================================
-- SAMPLE CUSTOMERS (10 records)
-- =========================================
INSERT INTO Customers (CustomerName, Phone, Email, Address) VALUES
('Ramesh Kumar', '9876543210', 'ramesh@gmail.com', 'Chennai'),
('Suresh Kumar', '9876543211', 'suresh@gmail.com', 'Coimbatore'),
('Arun Prakash', '9876543212', 'arun@gmail.com', 'Madurai'),
('Vignesh Kumar', '9876543213', 'vignesh@gmail.com', 'Salem'),
('Karthik Raj', '9876543214', 'karthik@gmail.com', 'Trichy'),
('Manoj Kumar', '9876543215', 'manoj@gmail.com', 'Erode'),
('Santhosh Kumar', '9876543216', 'santhosh@gmail.com', 'Tirunelveli'),
('Rahul Kumar', '9876543217', 'rahul@gmail.com', 'Vellore'),
('Praveen Kumar', '9876543218', 'praveen@gmail.com', 'Chengalpattu'),
('Ajay Kumar', '9876543219', 'ajay@gmail.com', 'Tambaram');

-- =========================================
-- SAMPLE PRODUCTS (10 records)
-- =========================================
INSERT INTO Products (ProductName, Price, Description) VALUES
('Laptop Basic Model', 45000, 'Entry level laptop'),
('Laptop Pro Model', 65000, 'High performance laptop'),
('Desktop Computer', 40000, 'Office desktop system'),
('Wireless Mouse', 500, 'Ergonomic wireless mouse'),
('Mechanical Keyboard', 2500, 'RGB mechanical keyboard'),
('Monitor 24 Inch', 12000, 'Full HD monitor'),
('Printer Laser', 15000, 'Laser printer for office'),
('External Hard Disk 1TB', 5000, 'Portable hard disk'),
('Web Camera HD', 2000, 'HD webcam for meetings'),
('UPS Power Backup', 3500, 'UPS for computer');

-- =========================================
-- SAMPLE TASKS (10 records)
-- =========================================
INSERT INTO Tasks (Title, Description, Status, DueDate, CustomerId, ProductId, AssignedTo) VALUES
('Call customer about laptop', 'Customer interested in laptop', 'Pending', '2026-03-25', 1, 1, 2),
('Follow up printer enquiry', 'Customer wants printer details', 'In Progress', '2026-03-26', 2, 7, 3),
('Demo for desktop computer', 'Provide product demo', 'Pending', '2026-03-27', 3, 3, 4),
('Send quotation', 'Quotation for laptop pro', 'Completed', '2026-03-20', 4, 2, 5),
('Customer interested in monitor', 'Explain monitor features', 'Pending', '2026-03-28', 5, 6, 6),
('Mouse enquiry follow up', 'Customer asking for mouse', 'Pending', '2026-03-29', 6, 4, 7),
('Keyboard sales call', 'Call customer about keyboard', 'In Progress', '2026-03-30', 7, 5, 8),
('Hard disk purchase follow up', 'Customer ready to buy', 'Completed', '2026-03-22', 8, 8, 9),
('Webcam enquiry', 'Customer needs webcam', 'Pending', '2026-03-31', 9, 9, 10),
('UPS enquiry follow up', 'Customer wants UPS', 'Pending', '2026-04-01', 10, 10, 2);

-- =========================================
-- SAMPLE TASK FOLLOWUPS (10 records)
-- =========================================
INSERT INTO TaskFollowups (TaskId, Notes, FollowupDate, SalesStatus, UserId) VALUES
(1, 'Customer will confirm tomorrow', '2026-03-19', 'Interested', 2),
(2, 'Customer asked for discount', '2026-03-19', 'Interested', 3),
(3, 'Demo scheduled', '2026-03-19', 'Interested', 4),
(4, 'Product already purchased', '2026-03-18', 'Purchased', 5),
(5, 'Customer wants more details', '2026-03-19', 'Interested', 6),
(6, 'Customer not responding', '2026-03-19', 'Not Interested', 7),
(7, 'Follow up again next week', '2026-03-19', 'Interested', 8),
(8, 'Customer completed purchase', '2026-03-18', 'Purchased', 9),
(9, 'Customer requested callback', '2026-03-19', 'Interested', 10),
(10, 'Customer comparing products', '2026-03-19', 'Interested', 2);

-- =========================================
-- DONE
-- =========================================

SELECT * FROM Tasks;


CREATE TABLE Orders
(
    OrderId INT AUTO_INCREMENT PRIMARY KEY,
    TaskId INT,
    ProductId INT,
    Quantity INT,
    Price DECIMAL(10,2),
    TotalAmount DECIMAL(10,2),
    OrderDate DATE,
    UserId INT
);
