CREATE PROCEDURE [Recipe].[usp_Recipe_Create]
    @RecipeName NVARCHAR(200),
    @Description NVARCHAR(500) = NULL,
    @Servings INT = NULL,
    @PrepTimeMinutes INT = NULL,
    @CookTimeMinutes INT = NULL,
    @IsActive BIT = 1,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Recipe].[Recipe] (RecipeName, Description, Servings, PrepTimeMinutes, CookTimeMinutes, IsActive, CreatedBy, CreateDate)
    VALUES (@RecipeName, @Description, @Servings, @PrepTimeMinutes, @CookTimeMinutes, @IsActive, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END
