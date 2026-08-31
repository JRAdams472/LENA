CREATE PROCEDURE [Wine].[usp_Type_Delete]
    @TypeID INT
AS
BEGIN
    DELETE FROM [Wine].[Type] WHERE TypeID = @TypeID;
END