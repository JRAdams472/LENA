CREATE PROCEDURE [Wine].[usp_Type_Create]
    @TypeName NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Type] (TypeName, Description, IsActive, CreatedBy, CreateDate)
    VALUES (@TypeName, @Description, @IsActive, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END