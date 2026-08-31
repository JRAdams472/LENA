CREATE PROCEDURE [Wine].[usp_Region_Create]
    @RegionName NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CountryID INT,
    @CreatedBy NVARCHAR(100),
    @CreateDate DATETIME2
AS
BEGIN
    INSERT INTO [Wine].[Region] (RegionName, Description, IsActive, CountryID, CreatedBy, CreateDate)
    VALUES (@RegionName, @Description, @IsActive, @CountryID, @CreatedBy, @CreateDate);
    SELECT CAST(SCOPE_IDENTITY() as int);
END