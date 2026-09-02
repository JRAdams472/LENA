CREATE TABLE [Inventory].[Item] (
    [ItemID] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(200) NOT NULL,
    [BrandID] INT NULL,
    [UPC12] NVARCHAR(12) NULL,
    [UPC14] NVARCHAR(14) NULL,
    [CategoryID] INT NOT NULL,
    [Unit] NVARCHAR(20) NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreateDate] DATETIME2 NOT NULL,
    [LastUpdatedBy] NVARCHAR(100) NULL,
    [LastUpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Item] PRIMARY KEY CLUSTERED ([ItemID] ASC),
    CONSTRAINT [UQ_Item_Name_BrandID] UNIQUE ([Name], [BrandID]),
    CONSTRAINT [UQ_Item_UPC12] UNIQUE ([UPC12]),
    CONSTRAINT [UQ_Item_UPC14] UNIQUE ([UPC14]),
    CONSTRAINT [FK_Item_Category] FOREIGN KEY ([CategoryID]) REFERENCES [Inventory].[Category] ([CategoryID]),
    CONSTRAINT [FK_Item_ItemBrand] FOREIGN KEY ([BrandID]) REFERENCES [Inventory].[ItemBrand] ([ItemBrandID])
);
