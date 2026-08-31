CREATE PROCEDURE [Wine].[usp_Bottle_ListAll]
AS
BEGIN
    SELECT * FROM [Wine].[Bottle] ORDER BY BottleNumber;
END