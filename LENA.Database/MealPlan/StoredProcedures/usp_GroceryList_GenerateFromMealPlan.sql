CREATE PROCEDURE [MealPlan].[usp_GroceryList_GenerateFromMealPlan]
    @MealPlanID INT = NULL,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    DECLARE @LastGeneratedDate DATETIME2 = (SELECT MAX(GeneratedDate) FROM [MealPlan].[GroceryList]);

    INSERT INTO [MealPlan].[GroceryList] (MealPlanID, GeneratedDate, CreatedBy, CreateDate)
    VALUES (@MealPlanID, @CreateDate, @CreatedBy, @CreateDate);

    DECLARE @GroceryListID INT = CAST(SCOPE_IDENTITY() AS INT);

    ;WITH PlanItems AS (
        SELECT ri.ItemID, SUM(ri.Quantity) AS TotalNeeded
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [Recipe].[Recipe] r ON s.RecipeID = r.RecipeID
        INNER JOIN [Recipe].[RecipeItem] ri ON r.RecipeID = ri.RecipeID
        WHERE s.MealPlanID = @MealPlanID
          AND ri.IsOptional = 0
        GROUP BY ri.ItemID

        UNION ALL

        SELECT ri.ItemID, SUM(ri.Quantity) AS TotalNeeded
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [Recipe].[Recipe] r ON s.RecipeID = r.RecipeID
        INNER JOIN [Recipe].[RecipeItem] ri ON r.RecipeID = ri.RecipeID
        INNER JOIN [MealPlan].[MealSlotItem] msi ON s.MealSlotID = msi.MealSlotID AND ri.ItemID = msi.ItemID
        WHERE s.MealPlanID = @MealPlanID
          AND ri.IsOptional = 1
          AND msi.IsFromRecipe = 1
        GROUP BY ri.ItemID

        UNION ALL

        SELECT msi.ItemID, SUM(msi.Quantity) AS TotalNeeded
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [MealPlan].[MealSlotItem] msi ON s.MealSlotID = msi.MealSlotID
        WHERE s.MealPlanID = @MealPlanID
          AND msi.IsFromRecipe = 0
        GROUP BY msi.ItemID
    ),
    NetNeeds AS (
        SELECT p.ItemID, i.Unit, SUM(p.TotalNeeded) - i.CurrentQuantity AS QuantityNeeded
        FROM PlanItems p
        INNER JOIN [Inventory].[Item] i ON p.ItemID = i.ItemID
        GROUP BY p.ItemID, i.CurrentQuantity, i.Unit
        HAVING SUM(p.TotalNeeded) - i.CurrentQuantity > 0
    )
    INSERT INTO [MealPlan].[GroceryListItem] (GroceryListID, ItemID, ManualItemName, QuantityNeeded, UnitOfMeasure, Source, IsChecked, CreatedBy, CreateDate)
    SELECT @GroceryListID, n.ItemID, NULL, n.QuantityNeeded, n.Unit, 'MealPlan', 0, @CreatedBy, @CreateDate
    FROM NetNeeds n;

    INSERT INTO [MealPlan].[GroceryListItem] (GroceryListID, ItemID, ManualItemName, QuantityNeeded, UnitOfMeasure, Source, IsChecked, CreatedBy, CreateDate)
    SELECT @GroceryListID, i.ItemID, NULL, ISNULL(NULLIF(i.MinQuantity, 0), 1), i.Unit, 'Depleted', 0, @CreatedBy, @CreateDate
    FROM [Inventory].[Item] i
    WHERE i.CurrentQuantity = 0
      AND (@LastGeneratedDate IS NULL OR i.LastUpdatedDate > @LastGeneratedDate)
      AND NOT EXISTS (
          SELECT 1
          FROM [MealPlan].[GroceryListItem] gli
          WHERE gli.GroceryListID = @GroceryListID
            AND gli.ItemID = i.ItemID
      );

    SELECT GroceryListID, MealPlanID, GeneratedDate, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[GroceryList]
    WHERE GroceryListID = @GroceryListID;

    SELECT GroceryListItemID, GroceryListID, ItemID, ManualItemName, QuantityNeeded, UnitOfMeasure, Source, IsChecked, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [MealPlan].[GroceryListItem]
    WHERE GroceryListID = @GroceryListID
    ORDER BY Source, ManualItemName;
END
