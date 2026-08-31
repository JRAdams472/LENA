CREATE PROCEDURE [Recipe].[usp_RecipeItem_Create]
    @RecipeID INT,
    @ItemID INT,
    @Quantity DECIMAL(10,2),
    @UnitOfMeasure NVARCHAR(20) = NULL,
    @Notes NVARCHAR(500) = NULL
AS
BEGIN
    MERGE [Recipe].[RecipeItem] AS target
    USING (SELECT @RecipeID, @ItemID, @Quantity, @UnitOfMeasure, @Notes) AS source (RecipeID, ItemID, Quantity, UnitOfMeasure, Notes)
    ON (target.RecipeID = source.RecipeID AND target.ItemID = source.ItemID)
    WHEN MATCHED THEN
        UPDATE SET Quantity = source.Quantity, UnitOfMeasure = source.UnitOfMeasure, Notes = source.Notes
    WHEN NOT MATCHED THEN
        INSERT (RecipeID, ItemID, Quantity, UnitOfMeasure, Notes)
        VALUES (source.RecipeID, source.ItemID, source.Quantity, source.UnitOfMeasure, source.Notes);
END
