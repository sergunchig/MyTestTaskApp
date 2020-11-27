DROP DATABASE IF EXISTS [testdatabase];
GO
CREATE DATABASE [testdatabase]
GO
USE [testdatabase];
GO

CREATE TABLE [Provider](
[id] INT IDENTITY PRIMARY KEY,
[Name] NVARCHAR(MAX) NOT NULL
);

CREATE TABLE [Order] (
[id] INT IDENTITY,
[Number] NVARCHAR(MAX) NOT NULL,
[Date] DATETIME2(7) NOT NULL,
[ProviderId] INT NOT NULL
CONSTRAINT Order_pk PRIMARY KEY (id)
CONSTRAINT ProviderId_fk FOREIGN KEY (ProviderId) REFERENCES [Provider](id)
);

CREATE TABLE [OrderItem](
[id] INT IDENTITY PRIMARY KEY,
[OrderId] INT NOT NULL,
[Name] NVARCHAR(MAX),
[Quantity] DECIMAL(18,3) NOT NULL,
[Unit] NVARCHAR(MAX) NOT NULL 
CONSTRAINT fk_OrderItem FOREIGN KEY (OrderId) REFERENCES [Order](id)
);
GO;
CREATE VIEW [OrderView] 
AS
SELECT [Order].Date, [Order].Number, [OrderItem].Name, [OrderItem].Quantity, [OrderItem].Unit, [Provider].Name AS ProviderName
FROM     [Order] INNER JOIN
                  [OrderItem] ON [Order].id = [OrderItem].OrderId INNER JOIN
                  [Provider] ON [Order].ProviderId = [Provider].id
GO;