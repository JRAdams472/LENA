SET NOCOUNT ON;
GO

IF OBJECT_ID('[Inventory].[ItemBrand]') IS NULL
BEGIN
    CREATE TABLE [Inventory].[ItemBrand] (
        [ItemBrandID] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(100) NOT NULL,
        CONSTRAINT [PK_ItemBrand] PRIMARY KEY CLUSTERED ([ItemBrandID] ASC),
        CONSTRAINT [UQ_ItemBrand_Name] UNIQUE ([Name])
    );
END
GO

INSERT INTO [Inventory].[ItemBrand] ([Name])
SELECT DISTINCT [Brand]
FROM [Inventory].[Item]
WHERE [Brand] IS NOT NULL AND [Brand] <> ''
  AND NOT EXISTS (
      SELECT 1
      FROM [Inventory].[ItemBrand] ib
      WHERE ib.[Name] = [Item].[Brand]
  );
GO

IF COL_LENGTH('[Inventory].[Item]', 'BrandID') IS NULL
BEGIN
    ALTER TABLE [Inventory].[Item] ADD [BrandID] INT NULL;
END
GO

UPDATE i
SET i.[BrandID] = ib.[ItemBrandID]
FROM [Inventory].[Item] i
LEFT JOIN [Inventory].[ItemBrand] ib ON ib.[Name] = i.[Brand]
WHERE i.[Brand] IS NOT NULL AND i.[Brand] <> '';
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_Item_ItemBrand')
BEGIN
    ALTER TABLE [Inventory].[Item] ADD CONSTRAINT [FK_Item_ItemBrand] FOREIGN KEY ([BrandID]) REFERENCES [Inventory].[ItemBrand] ([ItemBrandID]);
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Item_Name_Brand')
BEGIN
    ALTER TABLE [Inventory].[Item] DROP CONSTRAINT [UQ_Item_Name_Brand];
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'UQ_Item_Name_BrandID')
BEGIN
    ALTER TABLE [Inventory].[Item] ADD CONSTRAINT [UQ_Item_Name_BrandID] UNIQUE ([Name], [BrandID]);
END
GO

IF COL_LENGTH('[Inventory].[Item]', 'Brand') IS NOT NULL
BEGIN
    ALTER TABLE [Inventory].[Item] DROP COLUMN [Brand];
END
GO
