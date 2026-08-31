CREATE PROCEDURE [Wine].[usp_Region_Update]
    @RegionID INT,
    @RegionName NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @CountryID INT,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    UPDATE [Wine].[Region]
    SET RegionName = @RegionName, Description = @Description, IsActive = @IsActive, CountryID = @CountryID,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE RegionID = @RegionID;
END