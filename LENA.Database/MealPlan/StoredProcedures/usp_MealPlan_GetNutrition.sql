-- Serving model:
--   * A meal slot plans [MealSlot].[Servings] servings of its recipe, so a recipe ingredient
--     contributes (RecipeItem.Quantity / Recipe.Servings) * MealSlot.Servings.
--   * Ad-hoc meal slot items are absolute quantities for the slot and are not scaled again.
--   * A line quantity is only meaningful against [food_nutrients].[amount_per_serving] when it is
--     expressed in the item's inventory Unit, so lines carrying a different UnitOfMeasure are
--     excluded rather than summed on an incompatible basis.
CREATE PROCEDURE [MealPlan].[usp_MealPlan_GetNutrition]
    @MealPlanID INT
AS
BEGIN
    SET NOCOUNT ON;

    WITH SlotItems AS (
        -- Required recipe ingredients
        SELECT
            s.MealSlotID,
            s.DayOfWeek,
            s.MealType,
            ri.ItemID,
            ri.UnitOfMeasure,
            ri.Quantity / CAST(ISNULL(NULLIF(r.Servings, 0), 1) AS DECIMAL(10,2)) * s.Servings AS Quantity
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [Recipe].[Recipe] r ON s.RecipeID = r.RecipeID
        INNER JOIN [Recipe].[RecipeItem] ri ON r.RecipeID = ri.RecipeID
        WHERE s.MealPlanID = @MealPlanID
          AND ri.IsOptional = 0

        UNION ALL

        -- Selected optional recipe ingredients
        SELECT
            s.MealSlotID,
            s.DayOfWeek,
            s.MealType,
            ri.ItemID,
            ri.UnitOfMeasure,
            ri.Quantity / CAST(ISNULL(NULLIF(r.Servings, 0), 1) AS DECIMAL(10,2)) * s.Servings AS Quantity
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [Recipe].[Recipe] r ON s.RecipeID = r.RecipeID
        INNER JOIN [Recipe].[RecipeItem] ri ON r.RecipeID = ri.RecipeID
        INNER JOIN [MealPlan].[MealSlotItem] msi ON s.MealSlotID = msi.MealSlotID AND ri.ItemID = msi.ItemID
        WHERE s.MealPlanID = @MealPlanID
          AND ri.IsOptional = 1
          AND msi.IsFromRecipe = 1

        UNION ALL

        -- Ad-hoc additional items
        SELECT
            s.MealSlotID,
            s.DayOfWeek,
            s.MealType,
            msi.ItemID,
            msi.UnitOfMeasure,
            msi.Quantity
        FROM [MealPlan].[MealSlot] s
        INNER JOIN [MealPlan].[MealSlotItem] msi ON s.MealSlotID = msi.MealSlotID
        WHERE s.MealPlanID = @MealPlanID
          AND msi.IsFromRecipe = 0
    ),
    Nutrients AS (
        SELECT
            si.MealSlotID,
            si.DayOfWeek,
            si.MealType,
            nt.nutrient_id AS NutrientId,
            nt.nutrient_name AS NutrientName,
            nt.unit_of_measure AS UnitOfMeasure,
            fn.amount_per_serving * si.Quantity AS Amount
        FROM SlotItems si
        INNER JOIN [Inventory].[Item] it ON si.ItemID = it.ItemID
        INNER JOIN [Inventory].[food_nutrients] fn ON si.ItemID = fn.food_id
        INNER JOIN [Inventory].[nutrient_types] nt ON fn.nutrient_id = nt.nutrient_id
        WHERE COALESCE(NULLIF(si.UnitOfMeasure, N''), it.Unit) = it.Unit
    )
    -- Per-slot / per-nutrient breakdown
    SELECT
        DayOfWeek,
        MealType,
        MealSlotID,
        NutrientId,
        NutrientName,
        UnitOfMeasure,
        CAST(SUM(Amount) AS DECIMAL(10,3)) AS Amount,
        CAST(0 AS BIT) AS IsDailyTotal
    FROM Nutrients
    GROUP BY DayOfWeek, MealType, MealSlotID, NutrientId, NutrientName, UnitOfMeasure

    UNION ALL

    -- Daily totals per nutrient
    SELECT
        DayOfWeek,
        NULL AS MealType,
        NULL AS MealSlotID,
        NutrientId,
        NutrientName,
        UnitOfMeasure,
        CAST(SUM(Amount) AS DECIMAL(10,3)) AS Amount,
        CAST(1 AS BIT) AS IsDailyTotal
    FROM Nutrients
    GROUP BY DayOfWeek, NutrientId, NutrientName, UnitOfMeasure

    ORDER BY DayOfWeek, IsDailyTotal, MealType, NutrientName;
END
