CREATE PROCEDURE [Recipe].[usp_Recipe_Create]
    @RecipeName NVARCHAR(200),
    @Description NVARCHAR(500) = NULL,
    @Servings INT = NULL,
    @PrepTimeMinutes INT = NULL,
    @CookTimeMinutes INT = NULL,
    @IsActive BIT = 1,
    @IsFavorite BIT = 0,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Recipe].[Recipe] (RecipeName, Description, Servings, PrepTimeMinutes, CookTimeMinutes, IsActive, IsFavorite, CreatedBy, CreateDate)
    VALUES (@RecipeName, @Description, @Servings, @PrepTimeMinutes, @CookTimeMinutes, @IsActive, @IsFavorite, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
