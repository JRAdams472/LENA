SET NOCOUNT ON;

IF OBJECT_ID('tempdb..#GroceryImport') IS NOT NULL
    DROP TABLE #GroceryImport;

CREATE TABLE #GroceryImport (
    grp_id NVARCHAR(50),
    upc14 NVARCHAR(14),
    upc12 NVARCHAR(12),
    brand NVARCHAR(100),
    brandid INT,
    name NVARCHAR(200)
);

BULK INSERT #GroceryImport
FROM '/tmp/GroceryItems.csv'
WITH (
    DATAFILETYPE = 'widechar',
    FIRSTROW = 2,
    FIELDTERMINATOR = '\t',
    ROWTERMINATOR = '\n'
);

UPDATE #GroceryImport
SET
    brand = LTRIM(RTRIM(REPLACE(brand, '"', ''))),
    name = LTRIM(RTRIM(REPLACE(name, '"', '')));

WHILE 1 = 1
BEGIN
    UPDATE #GroceryImport
    SET
        name = LTRIM(RTRIM(STUFF(name, 1, LEN(brand) + 1, '')))
    WHERE brand <> ''
      AND name LIKE brand + ' %';

    IF @@ROWCOUNT = 0 BREAK;
END

INSERT INTO [Inventory].[ItemBrand] ([Name])
SELECT DISTINCT g.brand
FROM #GroceryImport g
WHERE g.brand <> ''
  AND NOT EXISTS (
      SELECT 1
      FROM [Inventory].[ItemBrand] ib
      WHERE ib.[Name] = g.brand
  );

UPDATE g
SET
    g.brandid = ib.[ItemBrandID]
FROM #GroceryImport g
LEFT JOIN [Inventory].[ItemBrand] ib ON ib.[Name] = g.brand;

WITH Deduplicated AS (
    SELECT
        g.*,
        ROW_NUMBER() OVER (PARTITION BY g.upc14 ORDER BY (SELECT 1)) AS rn_upc14,
        ROW_NUMBER() OVER (PARTITION BY g.upc12 ORDER BY (SELECT 1)) AS rn_upc12,
        ROW_NUMBER() OVER (PARTITION BY g.name, g.brand ORDER BY (SELECT 1)) AS rn_name_brand
    FROM #GroceryImport g
)
DELETE FROM Deduplicated
WHERE rn_upc14 > 1 OR rn_upc12 > 1 OR rn_name_brand > 1;

INSERT INTO [Inventory].[Item] (
    [Name],
    [BrandID],
    [UPC12],
    [UPC14],
    [CategoryID],
    [Unit],
    [CurrentQuantity],
    [MinQuantity],
    [PurchaseDate],
    [ExpiryDate],
    [Notes],
    [IsFavorite],
    [CreatedBy],
    [CreateDate],
    [LastUpdatedBy],
    [LastUpdatedDate]
)
SELECT
    LEFT(g.name, 200),
    g.brandid,
    NULLIF(g.upc12, ''),
    NULLIF(g.upc14, ''),
    15,                          -- 'Other' category
    'ea',                        -- default unit
    0,                           -- quantity
    NULL,                        -- min quantity
    GETUTCDATE(),
    NULL,                        -- expiry date
    NULL,                        -- notes
    0,                           -- is favorite
    'System',
    GETUTCDATE(),
    NULL,
    NULL
FROM #GroceryImport g
WHERE g.name IS NOT NULL
  AND LTRIM(RTRIM(g.name)) <> ''
  AND NOT EXISTS (
      SELECT 1
      FROM [Inventory].[Item] i
      WHERE (i.[Name] = g.name AND (i.[BrandID] = g.brandid OR (i.[BrandID] IS NULL AND g.brandid IS NULL)))
         OR (g.upc12 <> '' AND i.[UPC12] = g.upc12)
         OR (g.upc14 <> '' AND i.[UPC14] = g.upc14)
         OR (g.upc12 = '' AND i.[UPC12] IS NULL)
         OR (g.upc14 = '' AND i.[UPC14] IS NULL)
  );
