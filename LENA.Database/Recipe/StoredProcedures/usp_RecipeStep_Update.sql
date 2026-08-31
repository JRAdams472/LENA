CREATE PROCEDURE [Recipe].[usp_RecipeStep_Update]
    @RecipeStepID INT,
    @RecipeID INT,
    @StepNumber INT,
    @Instruction NVARCHAR(MAX),
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;
    SET XACT_ABORT ON;

    BEGIN TRANSACTION;

    UPDATE [Recipe].[RecipeStep]
    SET Instruction = @Instruction,
        LastUpdatedBy = @LastUpdatedBy,
        LastUpdatedDate = @LastUpdatedDate
    WHERE RecipeStepID = @RecipeStepID
      AND RecipeID = @RecipeID;

    DECLARE @Affected INT = @@ROWCOUNT;

    IF @Affected > 0
    BEGIN
        -- Move the step to the requested position and compact the rest of the
        -- recipe, writing negative numbers first so the unique constraint holds.
        WITH Ordered AS (
            SELECT RecipeStepID,
                   ROW_NUMBER() OVER (
                       ORDER BY CASE WHEN RecipeStepID = @RecipeStepID THEN @StepNumber ELSE StepNumber END,
                                CASE WHEN RecipeStepID = @RecipeStepID THEN 0 ELSE 1 END,
                                RecipeStepID) AS NewStepNumber
            FROM [Recipe].[RecipeStep]
            WHERE RecipeID = @RecipeID
        )
        UPDATE s
        SET StepNumber = -o.NewStepNumber
        FROM [Recipe].[RecipeStep] s
        INNER JOIN Ordered o ON o.RecipeStepID = s.RecipeStepID;

        UPDATE [Recipe].[RecipeStep]
        SET StepNumber = -StepNumber
        WHERE RecipeID = @RecipeID
          AND StepNumber < 0;
    END

    COMMIT TRANSACTION;

    SELECT @Affected;
END
