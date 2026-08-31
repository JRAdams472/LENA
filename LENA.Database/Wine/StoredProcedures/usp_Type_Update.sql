CREATE PROCEDURE [Wine].[usp_Type_Update]
    @TypeID INT,
    @TypeName NVARCHAR(100),
    @Description NVARCHAR(200) = NULL,
    @IsActive BIT = 1,
    @LastUpdatedBy NVARCHAR(100) = NULL,
    @LastUpdatedDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [Wine].[Type]
    SET TypeName = @TypeName, Description = @Description, IsActive = @IsActive,
        LastUpdatedBy = @LastUpdatedBy, LastUpdatedDate = @LastUpdatedDate
    WHERE TypeID = @TypeID;

    SELECT @@ROWCOUNT;
END
