CREATE PROCEDURE [Wine].[usp_Bottle_GetAllByTypeId]
    @TypeId INT
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] WHERE TypeID = @TypeId ORDER BY BottleNumber;
END