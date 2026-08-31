CREATE PROCEDURE [Wine].[usp_Vintage_Update]
    @VintageID INT,
    @Year INT,
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [Wine].[Vintage]
    SET Year = @Year, Description = @Description, IsActive = @IsActive,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE VintageID = @VintageID;

    SELECT @@ROWCOUNT;
END
