CREATE TABLE [Inventory].[ItemBrand] (
    [ItemBrandID] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    CONSTRAINT [PK_ItemBrand] PRIMARY KEY CLUSTERED ([ItemBrandID] ASC),
    CONSTRAINT [UQ_ItemBrand_Name] UNIQUE ([Name])
);
