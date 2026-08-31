CREATE PROCEDURE [Recipe].[usp_RecipeStep_Create]
    @RecipeID INT,
    @StepNumber INT,
    @Instruction NVARCHAR(MAX),
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Recipe].[RecipeStep] (RecipeID, StepNumber, Instruction, CreatedBy, CreateDate)
    VALUES (@RecipeID, @StepNumber, @Instruction, @CreatedBy, @CreateDate);
END
