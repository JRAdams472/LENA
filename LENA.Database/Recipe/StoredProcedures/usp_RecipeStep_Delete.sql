CREATE PROCEDURE [Recipe].[usp_RecipeStep_Delete]
    @RecipeStepID INT,
    @RecipeID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    DECLARE @Deleted TABLE (RecipeID INT, StepNumber INT);

    DELETE FROM [Recipe].[RecipeStep]
    OUTPUT deleted.RecipeID, deleted.StepNumber INTO @Deleted
    WHERE RecipeStepID = @RecipeStepID
      AND (@RecipeID IS NULL OR RecipeID = @RecipeID);

    DECLARE @Affected INT = @@ROWCOUNT;

    IF @Affected > 0
    BEGIN
        DECLARE @OwningRecipeID INT, @DeletedStepNumber INT;
        SET @OwningRecipeID = (SELECT TOP (1) RecipeID FROM @Deleted);
        SET @DeletedStepNumber = (SELECT TOP (1) StepNumber FROM @Deleted);

        UPDATE [Recipe].[RecipeStep]
        SET StepNumber = -(StepNumber - 1)
        WHERE RecipeID = @OwningRecipeID
          AND StepNumber > @DeletedStepNumber;

        UPDATE [Recipe].[RecipeStep]
        SET StepNumber = -StepNumber
        WHERE RecipeID = @OwningRecipeID
          AND StepNumber < 0;
    END

    COMMIT TRANSACTION;

    SELECT @Affected;
END
