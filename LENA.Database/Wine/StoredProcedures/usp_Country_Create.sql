CREATE PROCEDURE [Wine].[usp_Country_Create]
    @CountryName NVARCHAR(100),
    @ISOCode NVARCHAR(10),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Country] (CountryName, ISOCode, Description, IsActive, CreatedBy, CreateDate)
    VALUES (@CountryName, @ISOCode, @Description, @IsActive, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END