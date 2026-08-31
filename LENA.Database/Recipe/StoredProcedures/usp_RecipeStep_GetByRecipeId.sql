CREATE PROCEDURE [Recipe].[usp_RecipeStep_GetByRecipeId]
    @RecipeID INT
AS
BEGIN
    SELECT RecipeStepID, RecipeID, StepNumber, Instruction, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [Recipe].[RecipeStep]
    WHERE RecipeID = @RecipeID
    ORDER BY StepNumber;
END
