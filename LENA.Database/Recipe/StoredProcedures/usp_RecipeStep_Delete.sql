CREATE PROCEDURE [Recipe].[usp_RecipeStep_Delete]
    @RecipeStepID INT
AS
BEGIN
    DELETE FROM [Recipe].[RecipeStep] WHERE RecipeStepID = @RecipeStepID;
END
