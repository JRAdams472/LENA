CREATE PROCEDURE [Recipe].[usp_UserRecipePreference_SetFavorite]
    @UserID INT,
    @RecipeID INT,
    @IsFavorite BIT,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2,
    @LastUpdatedBy NVARCHAR(100),
    @LastUpdatedDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    IF @IsFavorite = 1
    BEGIN
        MERGE [Recipe].[UserRecipePreference] AS target
        USING (VALUES (@UserID, @RecipeID, @IsFavorite)) AS source (UserID, RecipeID, IsFavorite)
        ON target.UserID = source.UserID AND target.RecipeID = source.RecipeID
        WHEN MATCHED THEN
            UPDATE SET IsFavorite = source.IsFavorite,
                       LastUpdatedBy = @LastUpdatedBy,
                       LastUpdatedDate = @LastUpdatedDate
        WHEN NOT MATCHED THEN
            INSERT (UserID, RecipeID, IsFavorite, CreatedBy, CreateDate)
            VALUES (source.UserID, source.RecipeID, source.IsFavorite, @CreatedBy, @CreateDate);
    END
    ELSE
    BEGIN
        DELETE FROM [Recipe].[UserRecipePreference] WHERE UserID = @UserID AND RecipeID = @RecipeID;
    END
END
