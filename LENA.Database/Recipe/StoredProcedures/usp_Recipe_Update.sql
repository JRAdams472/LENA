CREATE PROCEDURE [Recipe].[usp_Recipe_Update]
    @RecipeID INT,
    @RecipeName NVARCHAR(200),
    @Description NVARCHAR(500) = NULL,
    @Servings INT = NULL,
    @PrepTimeMinutes INT = NULL,
    @CookTimeMinutes INT = NULL,
    @IsActive BIT = 1,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [Recipe].[Recipe]
    SET RecipeName = @RecipeName,
        Description = @Description,
        Servings = @Servings,
        PrepTimeMinutes = @PrepTimeMinutes,
        CookTimeMinutes = @CookTimeMinutes,
        IsActive = @IsActive,
        LastUpdatedBy = @LastUpdatedBy,
        LastUpdatedDate = @LastUpdatedDate
    WHERE RecipeID = @RecipeID;

    SELECT @@ROWCOUNT;
END
