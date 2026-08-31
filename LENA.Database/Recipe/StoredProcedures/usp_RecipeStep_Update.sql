CREATE PROCEDURE [Recipe].[usp_RecipeStep_Update]
    @RecipeStepID INT,
    @RecipeID INT,
    @StepNumber INT,
    @Instruction NVARCHAR(MAX),
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    UPDATE [Recipe].[RecipeStep]
    SET RecipeID = @RecipeID,
        StepNumber = @StepNumber,
        Instruction = @Instruction,
        LastUpdatedBy = @LastUpdatedBy,
        LastUpdatedDate = @LastUpdatedDate
    WHERE RecipeStepID = @RecipeStepID;
END
