-- Quantity model:
--   * A meal slot plans [MealSlot].[Servings] servings of its recipe, so a recipe ingredient
--     contributes (RecipeItem.Quantity / Recipe.Servings) * MealSlot.Servings.
--   * Ad-hoc meal slot items are absolute quantities for the slot.
--   * Quantities are aggregated per (ItemID, unit of measure). A line's own UnitOfMeasure wins,
--     falling back to the item's inventory Unit, so lines expressed in different units are
--     listed separately instead of being summed into a meaningless total. On-hand inventory is
--     only netted off the group that is expressed in the item's inventory Unit.
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
        SELECT ri.ItemID, ri.UnitOfMeasure, SUM(ri.Quantity / CAST(ISNULL(NULLIF(r.Servings, 0), 1) AS DECIMAL(10, 2)) * s.Servings) AS TotalNeeded
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [Recipe].[Recipe] r ON s.RecipeID = r.RecipeID
        INNER JOIN [Recipe].[RecipeItem] ri ON r.RecipeID = ri.RecipeID
        WHERE s.MealPlanID = @MealPlanID
          AND ri.IsOptional = 0
        GROUP BY ri.ItemID, ri.UnitOfMeasure

        UNION ALL

        SELECT ri.ItemID, ri.UnitOfMeasure, SUM(ri.Quantity / CAST(ISNULL(NULLIF(r.Servings, 0), 1) AS DECIMAL(10, 2)) * s.Servings) AS TotalNeeded
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [Recipe].[Recipe] r ON s.RecipeID = r.RecipeID
        INNER JOIN [Recipe].[RecipeItem] ri ON r.RecipeID = ri.RecipeID
        INNER JOIN [MealPlan].[MealSlotItem] msi ON s.MealSlotID = msi.MealSlotID AND ri.ItemID = msi.ItemID
        WHERE s.MealPlanID = @MealPlanID
          AND ri.IsOptional = 1
          AND msi.IsFromRecipe = 1
        GROUP BY ri.ItemID, ri.UnitOfMeasure

        UNION ALL

        SELECT msi.ItemID, msi.UnitOfMeasure, SUM(msi.Quantity) AS TotalNeeded
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [MealPlan].[MealSlotItem] msi ON s.MealSlotID = msi.MealSlotID
        WHERE s.MealPlanID = @MealPlanID
          AND msi.IsFromRecipe = 0
        GROUP BY msi.ItemID, msi.UnitOfMeasure
    ),
    NetNeeds AS (
        SELECT
            p.ItemID,
            COALESCE(NULLIF(p.UnitOfMeasure, N''), i.Unit) AS Unit,
            SUM(p.TotalNeeded)
                - CASE WHEN COALESCE(NULLIF(p.UnitOfMeasure, N''), i.Unit) = i.Unit THEN i.CurrentQuantity ELSE 0 END AS QuantityNeeded
        FROM PlanItems p
        INNER JOIN [Inventory].[Item] i ON p.ItemID = i.ItemID
        GROUP BY p.ItemID, i.CurrentQuantity, i.Unit, COALESCE(NULLIF(p.UnitOfMeasure, N''), i.Unit)
        HAVING SUM(p.TotalNeeded)
            - CASE WHEN COALESCE(NULLIF(p.UnitOfMeasure, N''), i.Unit) = i.Unit THEN i.CurrentQuantity ELSE 0 END > 0
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

    SELECT gli.GroceryListItemID, gli.GroceryListID, gli.ItemID, i.Name AS ItemName, gli.ManualItemName,
           gli.QuantityNeeded, gli.UnitOfMeasure, gli.Source, gli.IsChecked,
           gli.CreatedBy, gli.CreateDate, gli.LastUpdatedBy, gli.LastUpdatedDate
    FROM [MealPlan].[GroceryListItem] gli
    LEFT JOIN [Inventory].[Item] i ON gli.ItemID = i.ItemID
    WHERE gli.GroceryListID = @GroceryListID
    ORDER BY gli.Source, COALESCE(i.Name, gli.ManualItemName);
END
