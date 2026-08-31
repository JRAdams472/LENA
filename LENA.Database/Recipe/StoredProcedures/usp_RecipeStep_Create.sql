CREATE PROCEDURE [Recipe].[usp_RecipeStep_Create]
    @RecipeID INT,
    @StepNumber INT,
    @Instruction NVARCHAR(MAX),
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    -- Make room for the new step; the two-phase negative renumber keeps
    -- UQ_RecipeStep_RecipeID_StepNumber satisfied at every statement boundary.
    UPDATE [Recipe].[RecipeStep]
    SET StepNumber = -(StepNumber + 1)
    WHERE RecipeID = @RecipeID
      AND StepNumber >= @StepNumber;

    UPDATE [Recipe].[RecipeStep]
    SET StepNumber = -StepNumber
    WHERE RecipeID = @RecipeID
      AND StepNumber < 0;

    INSERT INTO [Recipe].[RecipeStep]
        (RecipeID, StepNumber, Instruction, CreatedBy, CreateDate)
    VALUES
        (@RecipeID, @StepNumber, @Instruction, @CreatedBy, @CreateDate);

    DECLARE @RecipeStepID INT = CAST(SCOPE_IDENTITY() AS INT);

    COMMIT TRANSACTION;

    SELECT @RecipeStepID;
END
