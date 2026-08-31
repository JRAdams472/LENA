CREATE PROCEDURE [Wine].[usp_Vintage_Create]
    @Year INT,
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Vintage] (Year, Description, IsActive, CreatedBy, CreateDate)
    VALUES (@Year, @Description, @IsActive, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END