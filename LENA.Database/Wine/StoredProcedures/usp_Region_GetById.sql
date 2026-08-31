CREATE PROCEDURE [Wine].[usp_Region_GetById]
    @Id INT
AS
BEGIN
    SELECT * FROM [Wine].[Region] WHERE RegionID = @Id;
END