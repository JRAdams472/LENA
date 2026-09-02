SET NOCOUNT ON;
SET XACT_ABORT ON;

-- Phase 7: verify the multi-user data model migration is complete and consistent.
-- This migration is intentionally idempotent and must return without error when run.

DECLARE @Issues TABLE (Issue NVARCHAR(500));

-- 1. No scoped row may have a NULL UserID
INSERT INTO @Issues
SELECT 'MealPlan.MealPlan has NULL UserID'
FROM [MealPlan].[MealPlan]
WHERE [UserID] IS NULL
HAVING COUNT(*) > 0;

INSERT INTO @Issues
SELECT 'MealPlan.GroceryList has NULL UserID'
FROM [MealPlan].[GroceryList]
WHERE [UserID] IS NULL
HAVING COUNT(*) > 0;

INSERT INTO @Issues
SELECT 'Recipe.UserRecipePreference has NULL UserID'
FROM [Recipe].[UserRecipePreference]
WHERE [UserID] IS NULL
HAVING COUNT(*) > 0;

INSERT INTO @Issues
SELECT 'Inventory.UserItem has NULL UserID'
FROM [Inventory].[UserItem]
WHERE [UserID] IS NULL
HAVING COUNT(*) > 0;

INSERT INTO @Issues
SELECT 'Wine.UserBottle has NULL UserID'
FROM [Wine].[UserBottle]
WHERE [UserID] IS NULL
HAVING COUNT(*) > 0;

-- 2. Every catalog Item must have at least one UserItem holding after the backfill
IF EXISTS (
    SELECT 1
    FROM [Inventory].[Item] i
    WHERE NOT EXISTS (SELECT 1 FROM [Inventory].[UserItem] ui WHERE ui.ItemID = i.ItemID)
)
    INSERT INTO @Issues SELECT 'Inventory.Item has rows with no matching Inventory.UserItem';

-- 3. Every catalog Bottle must have at least one UserBottle holding after the backfill
IF EXISTS (
    SELECT 1
    FROM [Wine].[Bottle] b
    WHERE NOT EXISTS (SELECT 1 FROM [Wine].[UserBottle] ub WHERE ub.BottleID = b.BottleID)
)
    INSERT INTO @Issues SELECT 'Wine.Bottle has rows with no matching Wine.UserBottle';

-- 4. Per-user columns must have been removed from the catalog tables
DECLARE @CatalogsWithLeftoverColumns TABLE (TableName NVARCHAR(261), ColumnName NVARCHAR(128));

INSERT INTO @CatalogsWithLeftoverColumns
SELECT 'Inventory.Item', N'CurrentQuantity' WHERE COL_LENGTH(N'[Inventory].[Item]', N'CurrentQuantity') IS NOT NULL
UNION ALL
SELECT 'Inventory.Item', N'MinQuantity' WHERE COL_LENGTH(N'[Inventory].[Item]', N'MinQuantity') IS NOT NULL
UNION ALL
SELECT 'Inventory.Item', N'PurchaseDate' WHERE COL_LENGTH(N'[Inventory].[Item]', N'PurchaseDate') IS NOT NULL
UNION ALL
SELECT 'Inventory.Item', N'ExpiryDate' WHERE COL_LENGTH(N'[Inventory].[Item]', N'ExpiryDate') IS NOT NULL
UNION ALL
SELECT 'Inventory.Item', N'Notes' WHERE COL_LENGTH(N'[Inventory].[Item]', N'Notes') IS NOT NULL
UNION ALL
SELECT 'Inventory.Item', N'IsFavorite' WHERE COL_LENGTH(N'[Inventory].[Item]', N'IsFavorite') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'BottleNumber' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'BottleNumber') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'BottleSize' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'BottleSize') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'Quantity' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'Quantity') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'PurchaseDate' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'PurchaseDate') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'PurchasePrice' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'PurchasePrice') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'StorageTemp' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'StorageTemp') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'Location' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'Location') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'Notes' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'Notes') IS NOT NULL
UNION ALL
SELECT 'Wine.Bottle', N'IsFavorite' WHERE COL_LENGTH(N'[Wine].[Bottle]', N'IsFavorite') IS NOT NULL
UNION ALL
SELECT 'Recipe.Recipe', N'IsFavorite' WHERE COL_LENGTH(N'[Recipe].[Recipe]', N'IsFavorite') IS NOT NULL;

INSERT INTO @Issues
SELECT 'Leftover per-user column: ' + TableName + '.' + ColumnName
FROM @CatalogsWithLeftoverColumns;

-- 5. All scoped procedures should accept @UserID (best-effort compile-time check via sys.parameters)
DECLARE @UserIDProcs TABLE (ProcName NVARCHAR(520));
INSERT INTO @UserIDProcs
SELECT QUOTENAME(OBJECT_SCHEMA_NAME(p.object_id)) + N'.' + QUOTENAME(OBJECT_NAME(p.object_id))
FROM sys.parameters p
WHERE p.name = '@UserID'
  AND OBJECT_SCHEMA_NAME(p.object_id) IN (N'Inventory', N'Wine', N'MealPlan', N'Recipe');

IF EXISTS (
    SELECT 1
    FROM sys.procedures p
    WHERE OBJECT_SCHEMA_NAME(p.object_id) IN (N'Inventory', N'Wine', N'MealPlan', N'Recipe')
      AND p.name LIKE 'usp_%'
      AND p.name NOT IN (
          -- Reference-data procs and other intentionally shared procs that do not require UserID
          'usp_ItemFlavorProfile_GetAllActive', 'usp_ItemNutrientType_GetAllActive'
      )
      AND NOT EXISTS (
          SELECT 1 FROM sys.parameters sp
          WHERE sp.object_id = p.object_id AND sp.name = N'@UserID'
      )
      AND NOT EXISTS (
          SELECT 1 FROM sys.parameters sp
          WHERE sp.object_id = p.object_id AND sp.name = N'@CreatedBy'
             AND OBJECT_SCHEMA_NAME(p.object_id) = N'MealPlan'
             AND p.name LIKE 'usp_GroceryList%'
      )
)
    INSERT INTO @Issues SELECT 'Scoped stored procedure missing @UserID parameter';

-- Report any issues and fail the migration
IF EXISTS (SELECT 1 FROM @Issues)
BEGIN
    DECLARE @Message NVARCHAR(MAX) = 'Phase 7 verification failed: ' +
        (SELECT STRING_AGG(Issue, '; ') WITHIN GROUP (ORDER BY Issue) FROM @Issues);
    THROW 50000, @Message, 1;
END
