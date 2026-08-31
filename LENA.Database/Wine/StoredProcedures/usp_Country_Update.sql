CREATE PROCEDURE [Wine].[usp_Country_Update]
    @CountryID INT,
    @CountryName NVARCHAR(100),
    @ISOCode NVARCHAR(10),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [Wine].[Country]
    SET CountryName = @CountryName, ISOCode = @ISOCode, Description = @Description, IsActive = @IsActive,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE CountryID = @CountryID;

    SELECT @@ROWCOUNT;
END
