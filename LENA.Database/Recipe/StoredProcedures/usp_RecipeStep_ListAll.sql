CREATE PROCEDURE [Recipe].[usp_RecipeStep_ListAll]
AS
BEGIN
    SELECT RecipeStepID, RecipeID, StepNumber, Instruction, CreatedBy, CreateDate, LastUpdatedBy, LastUpdatedDate
    FROM [Recipe].[RecipeStep];
END
