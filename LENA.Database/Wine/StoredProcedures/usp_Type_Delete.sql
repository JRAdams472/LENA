CREATE PROCEDURE [Wine].[usp_Type_Delete]
    @TypeID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [Wine].[Type] WHERE TypeID = @TypeID;

    SELECT @@ROWCOUNT;
END
