CREATE PROCEDURE [Recipe].[usp_RecipeItem_Create]
    @RecipeID INT,
    @ItemID INT,
    @Quantity DECIMAL(10,2),
    @UnitOfMeasure NVARCHAR(20) = NULL,
    @Notes NVARCHAR(500) = NULL,
    @IsOptional BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    MERGE [Recipe].[RecipeItem] AS target
    USING (SELECT @RecipeID, @ItemID, @Quantity, @UnitOfMeasure, @Notes, @IsOptional)
        AS source (RecipeID, ItemID, Quantity, UnitOfMeasure, Notes, IsOptional)
    ON (target.RecipeID = source.RecipeID AND target.ItemID = source.ItemID)
    WHEN MATCHED THEN
        UPDATE SET Quantity = source.Quantity,
                   UnitOfMeasure = source.UnitOfMeasure,
                   Notes = source.Notes,
                   IsOptional = source.IsOptional
    WHEN NOT MATCHED THEN
        INSERT (RecipeID, ItemID, Quantity, UnitOfMeasure, Notes, IsOptional)
        VALUES (source.RecipeID, source.ItemID, source.Quantity, source.UnitOfMeasure, source.Notes, source.IsOptional)
    OUTPUT inserted.RecipeID, inserted.ItemID, inserted.Quantity, inserted.UnitOfMeasure, inserted.Notes, inserted.IsOptional;
END
