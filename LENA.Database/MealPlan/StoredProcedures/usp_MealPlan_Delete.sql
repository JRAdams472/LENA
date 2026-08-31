CREATE PROCEDURE [MealPlan].[usp_MealPlan_Delete]
    @MealPlanID INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [MealPlan].[MealPlan]
    WHERE MealPlanID = @MealPlanID;

    SELECT @@ROWCOUNT;
END
