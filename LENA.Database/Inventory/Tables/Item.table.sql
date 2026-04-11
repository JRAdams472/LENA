CREATE TABLE [Inventory].[Item] (
    [ItemID] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(200) NOT NULL,
    [Brand] NVARCHAR(100) NULL,
    [Barcode] NVARCHAR(50) NULL,
    [CategoryID] INT NOT NULL,
    [Unit] NVARCHAR(20) NOT NULL,
    [CurrentQuantity] DECIMAL(10,2) NOT NULL,
    [MinQuantity] DECIMAL(10,2) NULL,
    [PurchaseDate] DATETIME2 NOT NULL,
    [ExpiryDate] DATETIME2 NULL,
    [Notes] NVARCHAR(500) NULL,
    [IsFavorite] BIT DEFAULT 0 NOT NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([ItemID] ASC),
    CONSTRAINT [UQ_Item_Name_Brand] UNIQUE ([Name], [Brand]),
    CONSTRAINT [UQ_Item_Barcode] UNIQUE ([Barcode]),
    CONSTRAINT [FK_Item_Category] FOREIGN KEY ([CategoryID]) REFERENCES [Inventory].[Category] ([CategoryID])
);
